using Selqi.Domain.Catalog;
using Selqi.Domain.Operations;
using Selqi.Domain.Orders;

namespace Selqi.Application.Admin;

public interface IAdminRepository
{
    Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken ct = default);

    Task<Product?> GetProductAsync(Guid productId, CancellationToken ct = default);

    void AddProduct(Product product);

    Task<IReadOnlyList<Order>> GetOrdersAsync(CancellationToken ct = default);

    Task<Order?> GetOrderAsync(Guid orderId, CancellationToken ct = default);

    Task<IReadOnlyList<AuditLog>> GetAuditLogsAsync(int take, CancellationToken ct = default);

    void AddAuditLog(AuditLog auditLog);
    void AddOrderStatusHistory(OrderStatusHistory history);

    Task SaveChangesAsync(CancellationToken ct = default);

    // Store management
    Task<IReadOnlyList<Store>> GetStoresAsync(CancellationToken ct = default);

    Task<Store?> GetStoreAsync(Guid storeId, CancellationToken ct = default);

    void AddStore(Store store);

    void RemoveStore(Store store);
}
