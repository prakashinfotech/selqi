using Selqi.Application.Admin;
using Selqi.Domain.Catalog;
using Selqi.Domain.Operations;
using Selqi.Domain.Orders;
using Selqi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Selqi.Infrastructure.Admin;

internal sealed class EfAdminRepository(SelqiDbContext dbContext) : IAdminRepository
{
    public async Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken ct = default)
    {
        return await dbContext.Products
            .Include(product => product.Category)
            .Include(product => product.Brand)
            .Include(product => product.Inventory)
            .ThenInclude(inventory => inventory.Store)
            .OrderBy(product => product.Category.DisplayOrder)
            .ThenBy(product => product.Name)
            .ToListAsync(ct);
    }

    public Task<Product?> GetProductAsync(Guid productId, CancellationToken ct = default)
    {
        return dbContext.Products
            .Include(product => product.Category)
            .Include(product => product.Brand)
            .Include(product => product.Inventory)
            .ThenInclude(inventory => inventory.Store)
            .FirstOrDefaultAsync(product => product.Id == productId, ct);
    }

    public void AddProduct(Product product)
    {
        dbContext.Products.Add(product);
    }

    public async Task<IReadOnlyList<Order>> GetOrdersAsync(CancellationToken ct = default)
    {
        return await dbContext.Orders
            .Include(order => order.User)
            .Include(order => order.Items)
            .OrderByDescending(order => order.PlacedAtUtc)
            .ToListAsync(ct);
    }

    public Task<Order?> GetOrderAsync(Guid orderId, CancellationToken ct = default)
    {
        return dbContext.Orders
            .Include(order => order.User)
            .Include(order => order.Items)
            .FirstOrDefaultAsync(order => order.Id == orderId, ct);
    }

    public async Task<IReadOnlyList<AuditLog>> GetAuditLogsAsync(int take, CancellationToken ct = default)
    {
        return await dbContext.AuditLogs
            .AsNoTracking()
            .OrderByDescending(log => log.CreatedAtUtc)
            .Take(take)
            .ToListAsync(ct);
    }

    public void AddAuditLog(AuditLog auditLog)
    {
        dbContext.AuditLogs.Add(auditLog);
    }

    public void AddOrderStatusHistory(OrderStatusHistory history)
    {
        dbContext.OrderStatusHistories.Add(history);
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
    {
        return dbContext.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<Store>> GetStoresAsync(CancellationToken ct = default)
    {
        return await dbContext.Stores
            .Include(store => store.Inventory)
            .ThenInclude(inv => inv.Product)
            .OrderBy(store => store.Name)
            .ToListAsync(ct);
    }

    public Task<Store?> GetStoreAsync(Guid storeId, CancellationToken ct = default)
    {
        return dbContext.Stores
            .Include(store => store.Inventory)
            .ThenInclude(inv => inv.Product)
            .FirstOrDefaultAsync(store => store.Id == storeId, ct);
    }

    public void AddStore(Store store)
    {
        dbContext.Stores.Add(store);
    }

    public void RemoveStore(Store store)
    {
        dbContext.Stores.Remove(store);
    }
}
