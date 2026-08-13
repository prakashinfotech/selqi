using Selqi.Application.Basket;
using Selqi.Application.Constants;
using Selqi.Application.Identity;
using Selqi.Domain.Identity;
using Selqi.Domain.Orders;

namespace Selqi.Application.Orders;

public sealed class OrderService(
    IOrderRepository orders,
    IBasketService basketService,
    IBasketRepository basketRepo,
    ISavedCardRepository savedCards) : IOrderService
{
    public async Task<OrderDetailDto> PlaceOrderAsync(Guid userId, CheckoutRequest request, CancellationToken ct = default)
    {
        var basket = await basketRepo.GetByUserIdAsync(userId, ct);
        if (basket is null || !basket.Items.Any())
            throw new InvalidOperationException("Your basket is empty.");

        var subTotalForDelivery = basket.Items.Sum(i => i.UnitPrice * i.Quantity);
        var deliveryCharge = request.DeliveryOption switch
        {
            DeliveryConstants.OptionExpress => DeliveryConstants.ExpressCharge,
            DeliveryConstants.OptionStandard => subTotalForDelivery >= DeliveryConstants.FreeDeliveryThreshold
                ? 0m
                : DeliveryConstants.StandardCharge,
            _ => 0m
        };

        var subTotal = subTotalForDelivery;
        var orderNumber = GenerateOrderNumber();

        // Resolve payment details — either from saved card or from the submitted card fields
        string paymentRef;
        if (request.Payment.SavedCardId is not null)
        {
            var saved = await savedCards.GetByIdAsync(request.Payment.SavedCardId.Value, ct);
            if (saved is null || saved.UserId != userId)
                throw new InvalidOperationException("Saved card not found.");
            paymentRef = $"{PaymentConstants.MockSavedCardPrefix}{saved.Token}";
        }
        else
        {
            if (string.IsNullOrWhiteSpace(request.Payment.CardNumber))
                throw new InvalidOperationException("Payment details are required.");
            paymentRef = $"{PaymentConstants.MockCardPrefix}{Guid.NewGuid():N}";
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            OrderNumber = orderNumber,
            UserId = userId,
            Status = OrderStatus.Pending,
            SubTotal = subTotal,
            DeliveryCharge = deliveryCharge,
            Total = subTotal + deliveryCharge,
            DeliveryFirstName = request.DeliveryAddress.FirstName,
            DeliveryLastName = request.DeliveryAddress.LastName,
            DeliveryLine1 = request.DeliveryAddress.Line1,
            DeliveryLine2 = request.DeliveryAddress.Line2,
            DeliveryTown = request.DeliveryAddress.Town,
            DeliveryCounty = request.DeliveryAddress.County ?? string.Empty,
            DeliveryPostcode = request.DeliveryAddress.Postcode,
            DeliveryPhone = request.DeliveryAddress.Phone,
            PaymentReference = paymentRef,
            PaymentStatus = PaymentConstants.StatusAuthorised,
            PlacedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            Items = basket.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? string.Empty,
                ProductSku = i.Product?.Sku ?? string.Empty,
                ProductImageUrl = i.Product?.Images.OrderBy(img => img.DisplayOrder).FirstOrDefault()?.Url,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                LineTotal = i.UnitPrice * i.Quantity
            }).ToList()
        };

        order.StatusHistory.Add(new Domain.Orders.OrderStatusHistory
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            Status = OrderStatus.Pending,
            Note = "Order placed",
            ChangedAtUtc = DateTime.UtcNow
        });

        await orders.AddAsync(order, ct);
        await basketService.ClearAsync(userId, ct);

        // Persist new card if user opted in
        if (request.Payment.SaveCard && request.Payment.SavedCardId is null
            && request.Payment.CardNumber is not null
            && request.Payment.CardHolder is not null
            && request.Payment.ExpiryMonth is not null
            && request.Payment.ExpiryYear is not null)
        {
            var existingCards = await savedCards.GetByUserIdAsync(userId, ct);
            if (existingCards.All(c => c.IsDefault == false) || !existingCards.Any())
            {
                foreach (var ec in existingCards.Where(c => c.IsDefault)) ec.IsDefault = false;
            }
            savedCards.Add(new SavedCard
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CardholderName = request.Payment.CardHolder.Trim(),
                Last4 = request.Payment.CardNumber,
                Brand = request.Payment.CardBrand ?? PaymentConstants.DefaultCardBrand,
                ExpiryMonth = request.Payment.ExpiryMonth,
                ExpiryYear = request.Payment.ExpiryYear,
                Token = Guid.NewGuid().ToString("N"),
                IsDefault = !existingCards.Any(),
                CreatedAtUtc = DateTime.UtcNow
            });
        }

        await orders.SaveChangesAsync(ct);

        return ToDetail(order);
    }

    public async Task<OrderDetailDto> PlaceRazorpayOrderAsync(
        Guid userId,
        VerifyRazorpayPaymentRequest request,
        string razorpayPaymentId,
        CancellationToken ct = default)
    {
        var basket = await basketRepo.GetByUserIdAsync(userId, ct);
        if (basket is null || !basket.Items.Any())
            throw new InvalidOperationException("Your basket is empty.");

        var subTotal = basket.Items.Sum(i => i.UnitPrice * i.Quantity);
        var deliveryCharge = request.DeliveryOption switch
        {
            "express" => 5.95m,
            "standard" => subTotal >= 50m ? 0m : 3.95m,
            _ => 0m
        };

        var order = new Order
        {
            Id = Guid.NewGuid(),
            OrderNumber = GenerateOrderNumber(),
            UserId = userId,
            Status = OrderStatus.Confirmed,
            SubTotal = subTotal,
            DeliveryCharge = deliveryCharge,
            Total = subTotal + deliveryCharge,
            DeliveryFirstName = request.DeliveryAddress.FirstName,
            DeliveryLastName = request.DeliveryAddress.LastName,
            DeliveryLine1 = request.DeliveryAddress.Line1,
            DeliveryLine2 = request.DeliveryAddress.Line2,
            DeliveryTown = request.DeliveryAddress.Town,
            DeliveryCounty = request.DeliveryAddress.County ?? string.Empty,
            DeliveryPostcode = request.DeliveryAddress.Postcode,
            DeliveryPhone = request.DeliveryAddress.Phone,
            PaymentReference = $"rzp_{request.RazorpayOrderId}_{razorpayPaymentId}",
            PaymentMethod = "Razorpay",
            PaymentStatus = "Captured",
            PlacedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            Items = basket.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? string.Empty,
                ProductSku = i.Product?.Sku ?? string.Empty,
                ProductImageUrl = i.Product?.Images.OrderBy(img => img.DisplayOrder).FirstOrDefault()?.Url,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                LineTotal = i.UnitPrice * i.Quantity
            }).ToList()
        };

        order.StatusHistory.Add(new Domain.Orders.OrderStatusHistory
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            Status = OrderStatus.Confirmed,
            Note = "Payment captured via Razorpay",
            ChangedAtUtc = DateTime.UtcNow
        });

        await orders.AddAsync(order, ct);
        await basketService.ClearAsync(userId, ct);
        await orders.SaveChangesAsync(ct);

        return ToDetail(order);
    }

    public async Task<IReadOnlyList<OrderSummaryDto>> GetOrdersAsync(Guid userId, CancellationToken ct = default)
    {
        var list = await orders.GetByUserIdAsync(userId, ct);
        return list.Select(o => new OrderSummaryDto(
            o.Id, o.OrderNumber, o.Status.ToString(),
            o.Items.Sum(i => i.Quantity), o.Total, o.PlacedAtUtc)).ToList();
    }

    public async Task<OrderDetailDto> GetOrderAsync(Guid userId, string orderNumber, CancellationToken ct = default)
    {
        var order = await orders.GetByOrderNumberAsync(orderNumber, ct)
            ?? throw new KeyNotFoundException("Order not found.");
        if (order.UserId != userId)
            throw new UnauthorizedAccessException("Access denied.");
        return ToDetail(order);
    }

    private static OrderDetailDto ToDetail(Order o) => new(
        o.Id, o.OrderNumber, o.Status.ToString(),
        o.Items.Select(i => new OrderItemDto(
            i.ProductId, i.ProductName, i.ProductSku,
            i.ProductImageUrl, i.Quantity, i.UnitPrice, i.LineTotal)).ToList(),
        o.SubTotal, o.DeliveryCharge, o.Total,
        $"{o.DeliveryFirstName} {o.DeliveryLastName}",
        $"{o.DeliveryLine1}, {o.DeliveryTown}, {o.DeliveryPostcode}",
        o.PaymentReference, o.PlacedAtUtc,
        o.StatusHistory
            .OrderBy(h => h.ChangedAtUtc)
            .Select(h => new OrderStatusHistoryDto(h.Status.ToString(), h.ChangedAtUtc, h.Note))
            .ToList());

    private static string GenerateOrderNumber() =>
        $"{OrderConstants.NumberPrefix}-{DateTime.UtcNow:yyyy}-{Random.Shared.Next(OrderConstants.NumberRandomMin, OrderConstants.NumberRandomMax)}";
}
