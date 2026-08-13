using Selqi.Application.Constants;
using Selqi.Application.Email;
using Selqi.Application.Pdf;
using Selqi.Domain.Catalog;
using Selqi.Domain.Operations;
using Selqi.Domain.Orders;
using System.ComponentModel.DataAnnotations;

namespace Selqi.Application.Admin;

// Valid forward-only status transitions
file static class StatusTransitions
{
    private static readonly Dictionary<OrderStatus, OrderStatus[]> Allowed = new()
    {
        [OrderStatus.Pending]    = [OrderStatus.Confirmed, OrderStatus.Cancelled],
        [OrderStatus.Confirmed]  = [OrderStatus.Processing, OrderStatus.Cancelled],
        [OrderStatus.Processing] = [OrderStatus.Shipped, OrderStatus.Cancelled],
        [OrderStatus.Shipped]    = [OrderStatus.Delivered],
        [OrderStatus.Delivered]  = [],
        [OrderStatus.Cancelled]  = [],
    };

    public static bool IsAllowed(OrderStatus from, OrderStatus to) =>
        Allowed.TryGetValue(from, out var targets) && targets.Contains(to);

    public static string AllowedLabels(OrderStatus from) =>
        Allowed.TryGetValue(from, out var targets) && targets.Length > 0
            ? string.Join(", ", targets)
            : "none (terminal status)";
}

internal sealed class AdminService(IAdminRepository repository, IEmailService emailService, IInvoicePdfService pdfService) : IAdminService
{
    public async Task<AdminDashboardDto> GetDashboardAsync(CancellationToken ct = default)
    {
        var products = await repository.GetProductsAsync(ct);
        var orders = await repository.GetOrdersAsync(ct);
        return new AdminDashboardDto(
            products.Count(product => !product.IsDeleted),
            products.Count(product => !product.IsDeleted && product.Inventory.Sum(i => i.QuantityAvailable) == 0),
            orders.Count,
            orders.Count(order => DashboardConstants.OpenOrderStatuses.Contains(order.Status)),
            orders.Sum(order => order.Total),
            orders.OrderByDescending(order => order.PlacedAtUtc).Take(DashboardConstants.RecentOrdersCount).Select(ToOrderDto).ToArray());
    }

    public async Task<AdminProductDto> CreateProductAsync(
        CreateProductRequest request,
        AdminActor actor,
        CancellationToken ct = default)
    {
        if (request.WasPrice is not null && request.WasPrice <= request.Price)
            throw new InvalidOperationException("Was price must be greater than the active price.");

        var slug = GenerateSlug(request.Name);
        var product = new Product
        {
            Id = Guid.NewGuid(),
            CategoryId = request.CategoryId,
            BrandId = request.BrandId,
            Name = request.Name.Trim(),
            Slug = slug,
            Sku = request.Sku.Trim().ToUpperInvariant(),
            ShortDescription = request.ShortDescription.Trim(),
            FullDescription = string.IsNullOrWhiteSpace(request.FullDescription) ? null : request.FullDescription.Trim(),
            Price = request.Price,
            WasPrice = request.WasPrice,
            IsFeatured = request.IsFeatured,
            IsDeleted = false,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        // Create zero-stock inventory entry for every store
        var stores = await repository.GetStoresAsync(ct);
        foreach (var store in stores)
        {
            product.Inventory.Add(new Inventory
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                StoreId = store.Id,
                QuantityAvailable = 0,
                LowStockThreshold = InventoryConstants.DefaultLowStockThreshold,
                UpdatedAtUtc = DateTime.UtcNow
            });
        }

        repository.AddProduct(product);
        repository.AddAuditLog(new AuditLog
        {
            Id = Guid.NewGuid(),
            ActorUserId = actor.UserId,
            ActorEmail = actor.Email,
            Action = "ProductCreated",
            EntityType = "Product",
            EntityId = product.Id.ToString(),
            Summary = $"Created product '{product.Name}' (SKU: {product.Sku}).",
            CreatedAtUtc = DateTime.UtcNow
        });

        await repository.SaveChangesAsync(ct);

        var saved = await repository.GetProductAsync(product.Id, ct) ?? product;
        return ToProductDto(saved);
    }

    public async Task<IReadOnlyCollection<AdminProductDto>> GetProductsAsync(CancellationToken ct = default)
    {
        var products = await repository.GetProductsAsync(ct);
        return products.Select(ToProductDto).ToArray();
    }

    public async Task<AdminProductDto> UpdateProductAsync(
        Guid productId,
        UpdateAdminProductRequest request,
        AdminActor actor,
        CancellationToken ct = default)
    {
        if (request.WasPrice is not null && request.WasPrice <= request.Price)
            throw new InvalidOperationException("Was price must be greater than the active price.");

        var product = await repository.GetProductAsync(productId, ct)
            ?? throw new KeyNotFoundException("Product not found.");

        product.Name = request.Name.Trim();
        product.ShortDescription = request.ShortDescription.Trim();
        product.FullDescription = string.IsNullOrWhiteSpace(request.FullDescription)
            ? null
            : request.FullDescription.Trim();
        product.Price = request.Price;
        product.WasPrice = request.WasPrice;
        product.IsFeatured = request.IsFeatured;
        product.IsDeleted = request.IsDeleted;
        product.UpdatedAtUtc = DateTime.UtcNow;

        foreach (var stockRequest in request.Inventory)
        {
            var stock = product.Inventory.FirstOrDefault(i => i.StoreId == stockRequest.StoreId);
            if (stock is null) continue;

            if (stockRequest.QuantityAvailable > stock.QuantityAvailable)
                stock.LastRestockedAtUtc = DateTime.UtcNow;
            stock.QuantityAvailable = stockRequest.QuantityAvailable;
            stock.LowStockThreshold = stockRequest.LowStockThreshold;
            stock.UpdatedAtUtc = DateTime.UtcNow;
        }

        repository.AddAuditLog(new AuditLog
        {
            Id = Guid.NewGuid(),
            ActorUserId = actor.UserId,
            ActorEmail = actor.Email,
            Action = "ProductUpdated",
            EntityType = "Product",
            EntityId = product.Id.ToString(),
            Summary = $"Updated {product.Name} product details and stock.",
            CreatedAtUtc = DateTime.UtcNow
        });

        await repository.SaveChangesAsync(ct);
        return ToProductDto(product);
    }

    public async Task<IReadOnlyCollection<AdminOrderDto>> GetOrdersAsync(CancellationToken ct = default)
    {
        var orders = await repository.GetOrdersAsync(ct);
        return orders.Select(ToOrderDto).ToArray();
    }

    public async Task<AdminOrderDto> UpdateOrderStatusAsync(
        Guid orderId,
        UpdateOrderStatusRequest request,
        AdminActor actor,
        CancellationToken ct = default)
    {
        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var newStatus))
            throw new InvalidOperationException("Order status is not valid.");

        var order = await repository.GetOrderAsync(orderId, ct)
            ?? throw new KeyNotFoundException("Order not found.");

        var previousStatus = order.Status;

        if (!StatusTransitions.IsAllowed(previousStatus, newStatus))
            throw new InvalidOperationException(
                $"Cannot move order from '{previousStatus}' to '{newStatus}'. " +
                $"Allowed transitions: {StatusTransitions.AllowedLabels(previousStatus)}.");

        order.Status = newStatus;
        order.UpdatedAtUtc = DateTime.UtcNow;

        repository.AddOrderStatusHistory(new OrderStatusHistory
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            Status = newStatus,
            Note = $"Status updated by admin ({actor.Email})",
            ChangedAtUtc = DateTime.UtcNow
        });

        repository.AddAuditLog(new AuditLog
        {
            Id = Guid.NewGuid(),
            ActorUserId = actor.UserId,
            ActorEmail = actor.Email,
            Action = "OrderStatusUpdated",
            EntityType = "Order",
            EntityId = order.Id.ToString(),
            Summary = $"Changed order {order.OrderNumber} from {previousStatus} to {newStatus}.",
            CreatedAtUtc = DateTime.UtcNow
        });

        await repository.SaveChangesAsync(ct);

        _ = SendOrderStatusEmailAsync(order, newStatus);

        return ToOrderDto(order);
    }

    private async Task SendOrderStatusEmailAsync(Order order, OrderStatus newStatus)
    {
        try
        {
            var items = order.Items.Select(i => (i.ProductName, i.Quantity, i.UnitPrice)).ToList();
            var address = $"{order.DeliveryLine1}, {order.DeliveryTown}, {order.DeliveryPostcode}";

            switch (newStatus)
            {
                case OrderStatus.Confirmed:
                    var invoiceData = new InvoiceData(
                        order.OrderNumber,
                        order.PlacedAtUtc,
                        $"{order.User.FirstName} {order.User.LastName}",
                        order.User.Email,
                        address,
                        order.Items.Select(i => new InvoiceLineItem(i.ProductName, i.ProductSku, i.Quantity, i.UnitPrice)).ToArray(),
                        order.SubTotal,
                        order.DeliveryCharge,
                        order.Total);
                    var pdfBytes = pdfService.Generate(invoiceData);
                    var confirmBody = EmailTemplates.OrderConfirmed(order.User.FirstName, order.OrderNumber, order.Total, items, address);
                    await emailService.SendWithAttachmentAsync(
                        order.User.Email,
                        $"Order {order.OrderNumber} confirmed — your invoice",
                        confirmBody,
                        $"Invoice-{order.OrderNumber}.pdf",
                        pdfBytes);
                    break;

                case OrderStatus.Processing:
                    await emailService.SendAsync(
                        order.User.Email,
                        $"Your order {order.OrderNumber} is being processed",
                        EmailTemplates.OrderProcessing(order.User.FirstName, order.OrderNumber));
                    break;

                case OrderStatus.Shipped:
                    await emailService.SendAsync(
                        order.User.Email,
                        $"Your order {order.OrderNumber} has been shipped",
                        EmailTemplates.OrderShipped(order.User.FirstName, order.OrderNumber, order.Total, items, address));
                    break;

                case OrderStatus.Delivered:
                    await emailService.SendAsync(
                        order.User.Email,
                        $"Your order {order.OrderNumber} has been delivered",
                        EmailTemplates.OrderDelivered(order.User.FirstName, order.OrderNumber, order.Total));
                    break;

                case OrderStatus.Cancelled:
                    await emailService.SendAsync(
                        order.User.Email,
                        $"Your order {order.OrderNumber} has been cancelled",
                        EmailTemplates.OrderCancelled(order.User.FirstName, order.OrderNumber, order.Total));
                    break;
            }
        }
        catch
        {
            // Email failures must never surface as API errors
        }
    }

    public async Task<IReadOnlyCollection<AuditLogDto>> GetAuditLogsAsync(CancellationToken ct = default)
    {
        var logs = await repository.GetAuditLogsAsync(AuditConstants.DefaultLogLimit, ct);
        return logs.Select(log => new AuditLogDto(
            log.Id,
            log.ActorUserId,
            log.ActorEmail,
            log.Action,
            log.EntityType,
            log.EntityId,
            log.Summary,
            log.CreatedAtUtc)).ToArray();
    }

    public async Task<IReadOnlyCollection<AdminStoreDto>> GetStoresAsync(CancellationToken ct = default)
    {
        var stores = await repository.GetStoresAsync(ct);
        return stores.Select(ToStoreSummaryDto).ToArray();
    }

    public async Task<AdminStoreDetailDto> GetStoreAsync(Guid storeId, CancellationToken ct = default)
    {
        var store = await repository.GetStoreAsync(storeId, ct)
            ?? throw new KeyNotFoundException("Store not found.");
        return ToStoreDetailDto(store);
    }

    public async Task<AdminStoreDetailDto> CreateStoreAsync(
        CreateStoreRequest request,
        AdminActor actor,
        CancellationToken ct = default)
    {
        if (!Enum.TryParse<StoreType>(request.StoreType, true, out var storeType))
            throw new InvalidOperationException("Invalid store type.");

        var store = new Store
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            AddressLine1 = request.AddressLine1.Trim(),
            AddressLine2 = request.AddressLine2?.Trim(),
            Town = request.Town.Trim(),
            Postcode = request.Postcode.Trim().ToUpperInvariant(),
            Phone = request.Phone?.Trim(),
            StoreType = storeType,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        repository.AddStore(store);
        repository.AddAuditLog(new AuditLog
        {
            Id = Guid.NewGuid(),
            ActorUserId = actor.UserId,
            ActorEmail = actor.Email,
            Action = "StoreCreated",
            EntityType = "Store",
            EntityId = store.Id.ToString(),
            Summary = $"Created store '{store.Name}' in {store.Town} ({store.StoreType}).",
            CreatedAtUtc = DateTime.UtcNow
        });

        await repository.SaveChangesAsync(ct);
        return ToStoreDetailDto(store);
    }

    public async Task<AdminStoreDetailDto> UpdateStoreAsync(
        Guid storeId,
        UpdateStoreRequest request,
        AdminActor actor,
        CancellationToken ct = default)
    {
        if (!Enum.TryParse<StoreType>(request.StoreType, true, out var storeType))
            throw new InvalidOperationException("Invalid store type.");

        var store = await repository.GetStoreAsync(storeId, ct)
            ?? throw new KeyNotFoundException("Store not found.");

        store.Name = request.Name.Trim();
        store.AddressLine1 = request.AddressLine1.Trim();
        store.AddressLine2 = request.AddressLine2?.Trim();
        store.Town = request.Town.Trim();
        store.Postcode = request.Postcode.Trim().ToUpperInvariant();
        store.Phone = request.Phone?.Trim();
        store.StoreType = storeType;
        store.Latitude = request.Latitude;
        store.Longitude = request.Longitude;
        store.IsActive = request.IsActive;
        store.UpdatedAtUtc = DateTime.UtcNow;

        repository.AddAuditLog(new AuditLog
        {
            Id = Guid.NewGuid(),
            ActorUserId = actor.UserId,
            ActorEmail = actor.Email,
            Action = "StoreUpdated",
            EntityType = "Store",
            EntityId = store.Id.ToString(),
            Summary = $"Updated store '{store.Name}' — active: {store.IsActive}.",
            CreatedAtUtc = DateTime.UtcNow
        });

        await repository.SaveChangesAsync(ct);
        return ToStoreDetailDto(store);
    }

    public async Task UpdateStoreInventoryAsync(
        Guid storeId,
        UpdateStoreInventoryRequest request,
        AdminActor actor,
        CancellationToken ct = default)
    {
        var store = await repository.GetStoreAsync(storeId, ct)
            ?? throw new KeyNotFoundException("Store not found.");

        var inv = store.Inventory.FirstOrDefault(i => i.ProductId == request.ProductId)
            ?? throw new KeyNotFoundException("Inventory record not found for this store/product combination.");

        if (request.QuantityAvailable > inv.QuantityAvailable)
            inv.LastRestockedAtUtc = DateTime.UtcNow;
        inv.QuantityAvailable = request.QuantityAvailable;
        inv.LowStockThreshold = request.LowStockThreshold;
        inv.UpdatedAtUtc = DateTime.UtcNow;

        repository.AddAuditLog(new AuditLog
        {
            Id = Guid.NewGuid(),
            ActorUserId = actor.UserId,
            ActorEmail = actor.Email,
            Action = "InventoryUpdated",
            EntityType = "Inventory",
            EntityId = inv.Id.ToString(),
            Summary = $"Store '{store.Name}': set qty={request.QuantityAvailable}, threshold={request.LowStockThreshold} for product {request.ProductId}.",
            CreatedAtUtc = DateTime.UtcNow
        });

        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteStoreAsync(Guid storeId, AdminActor actor, CancellationToken ct = default)
    {
        var store = await repository.GetStoreAsync(storeId, ct)
            ?? throw new KeyNotFoundException("Store not found.");

        repository.AddAuditLog(new AuditLog
        {
            Id = Guid.NewGuid(),
            ActorUserId = actor.UserId,
            ActorEmail = actor.Email,
            Action = "StoreDeleted",
            EntityType = "Store",
            EntityId = store.Id.ToString(),
            Summary = $"Deleted store '{store.Name}' in {store.Town}.",
            CreatedAtUtc = DateTime.UtcNow
        });

        repository.RemoveStore(store);
        await repository.SaveChangesAsync(ct);
    }

    private static string GenerateSlug(string name)
    {
        var slug = name.ToLowerInvariant().Trim();
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-");
        slug = slug.Trim('-');
        var suffix = Guid.NewGuid().ToString("N")[..SlugConstants.SuffixLength];
        return $"{slug}-{suffix}";
    }

    private static AdminProductDto ToProductDto(Product product) => new(
        product.Id,
        product.Name,
        product.Slug,
        product.Sku,
        product.Category.Name,
        product.Brand.Name,
        product.ShortDescription,
        product.FullDescription,
        product.Price,
        product.WasPrice,
        product.IsFeatured,
        product.IsDeleted,
        product.Inventory.Sum(inventory => inventory.QuantityAvailable),
        product.UpdatedAtUtc,
        product.Inventory
            .OrderBy(inventory => inventory.Store.Name)
            .Select(inventory => new AdminInventoryDto(
                inventory.StoreId,
                inventory.Store.Name,
                inventory.Store.Town,
                inventory.QuantityAvailable,
                inventory.LowStockThreshold,
                inventory.QuantityAvailable > 0 && inventory.QuantityAvailable <= inventory.LowStockThreshold,
                inventory.LastRestockedAtUtc))
            .ToArray());

    private static AdminStoreDto ToStoreSummaryDto(Store store) => new(
        store.Id,
        store.Name,
        store.AddressLine1,
        store.AddressLine2,
        store.Town,
        store.Postcode,
        store.Phone,
        store.StoreType.ToString(),
        store.Latitude,
        store.Longitude,
        store.IsActive,
        store.Inventory.Sum(i => i.QuantityAvailable),
        store.Inventory.Count(i => i.QuantityAvailable > 0 && i.QuantityAvailable <= i.LowStockThreshold),
        store.CreatedAtUtc,
        store.UpdatedAtUtc);

    private static AdminStoreDetailDto ToStoreDetailDto(Store store) => new(
        store.Id,
        store.Name,
        store.AddressLine1,
        store.AddressLine2,
        store.Town,
        store.Postcode,
        store.Phone,
        store.StoreType.ToString(),
        store.Latitude,
        store.Longitude,
        store.IsActive,
        store.CreatedAtUtc,
        store.UpdatedAtUtc,
        store.Inventory
            .OrderBy(i => i.Product.Name)
            .Select(i => new AdminStoreInventoryItemDto(
                i.ProductId,
                i.Product.Name,
                i.Product.Sku,
                i.QuantityAvailable,
                i.LowStockThreshold,
                i.QuantityAvailable > 0 && i.QuantityAvailable <= i.LowStockThreshold,
                i.LastRestockedAtUtc))
            .ToArray());

    private static AdminOrderDto ToOrderDto(Order order) => new(
        order.Id,
        order.OrderNumber,
        order.Status.ToString(),
        $"{order.User.FirstName} {order.User.LastName}",
        order.User.Email,
        order.Items.Sum(item => item.Quantity),
        order.Total,
        order.PlacedAtUtc,
        order.UpdatedAtUtc);
}
