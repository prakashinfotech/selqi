namespace Selqi.Application.Admin;

public interface IAdminService
{
    Task<AdminDashboardDto> GetDashboardAsync(CancellationToken ct = default);

    Task<IReadOnlyCollection<AdminProductDto>> GetProductsAsync(CancellationToken ct = default);

    Task<AdminProductDto> CreateProductAsync(
        CreateProductRequest request,
        AdminActor actor,
        CancellationToken ct = default);

    Task<AdminProductDto> UpdateProductAsync(
        Guid productId,
        UpdateAdminProductRequest request,
        AdminActor actor,
        CancellationToken ct = default);

    Task<IReadOnlyCollection<AdminOrderDto>> GetOrdersAsync(CancellationToken ct = default);

    Task<AdminOrderDto> UpdateOrderStatusAsync(
        Guid orderId,
        UpdateOrderStatusRequest request,
        AdminActor actor,
        CancellationToken ct = default);

    Task<IReadOnlyCollection<AuditLogDto>> GetAuditLogsAsync(CancellationToken ct = default);

    // Store management
    Task<IReadOnlyCollection<AdminStoreDto>> GetStoresAsync(CancellationToken ct = default);

    Task<AdminStoreDetailDto> GetStoreAsync(Guid storeId, CancellationToken ct = default);

    Task<AdminStoreDetailDto> CreateStoreAsync(
        CreateStoreRequest request,
        AdminActor actor,
        CancellationToken ct = default);

    Task<AdminStoreDetailDto> UpdateStoreAsync(
        Guid storeId,
        UpdateStoreRequest request,
        AdminActor actor,
        CancellationToken ct = default);

    Task UpdateStoreInventoryAsync(
        Guid storeId,
        UpdateStoreInventoryRequest request,
        AdminActor actor,
        CancellationToken ct = default);

    Task DeleteStoreAsync(Guid storeId, AdminActor actor, CancellationToken ct = default);
}
