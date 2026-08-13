using Selqi.Application.Constants;
using System.ComponentModel.DataAnnotations;

namespace Selqi.Application.Admin;

public sealed record AdminDashboardDto(
    int ProductCount,
    int OutOfStockProductCount,
    int OrderCount,
    int OpenOrderCount,
    decimal TotalRevenue,
    IReadOnlyCollection<AdminOrderDto> RecentOrders);

public sealed record AdminProductDto(
    Guid Id,
    string Name,
    string Slug,
    string Sku,
    string CategoryName,
    string BrandName,
    string ShortDescription,
    string? FullDescription,
    decimal Price,
    decimal? WasPrice,
    bool IsFeatured,
    bool IsDeleted,
    int TotalStock,
    DateTime? UpdatedAtUtc,
    IReadOnlyCollection<AdminInventoryDto> Inventory);

public sealed record AdminInventoryDto(
    Guid StoreId,
    string StoreName,
    string Town,
    int QuantityAvailable,
    int LowStockThreshold,
    bool IsLowStock,
    DateTime? LastRestockedAtUtc);

public sealed record AdminStoreDto(
    Guid Id,
    string Name,
    string AddressLine1,
    string? AddressLine2,
    string Town,
    string Postcode,
    string? Phone,
    string StoreType,
    double? Latitude,
    double? Longitude,
    bool IsActive,
    int TotalStockItems,
    int LowStockProductCount,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record AdminStoreDetailDto(
    Guid Id,
    string Name,
    string AddressLine1,
    string? AddressLine2,
    string Town,
    string Postcode,
    string? Phone,
    string StoreType,
    double? Latitude,
    double? Longitude,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    IReadOnlyCollection<AdminStoreInventoryItemDto> Inventory);

public sealed record AdminStoreInventoryItemDto(
    Guid ProductId,
    string ProductName,
    string ProductSku,
    int QuantityAvailable,
    int LowStockThreshold,
    bool IsLowStock,
    DateTime? LastRestockedAtUtc);

public sealed record CreateStoreRequest(
    [Required, MaxLength(160)] string Name,
    [Required, MaxLength(250)] string AddressLine1,
    [MaxLength(250)] string? AddressLine2,
    [Required, MaxLength(120)] string Town,
    [Required, MaxLength(20)] string Postcode,
    [MaxLength(20)] string? Phone,
    [Required] string StoreType,
    double? Latitude,
    double? Longitude);

public sealed record UpdateStoreRequest(
    [Required, MaxLength(160)] string Name,
    [Required, MaxLength(250)] string AddressLine1,
    [MaxLength(250)] string? AddressLine2,
    [Required, MaxLength(120)] string Town,
    [Required, MaxLength(20)] string Postcode,
    [MaxLength(20)] string? Phone,
    [Required] string StoreType,
    double? Latitude,
    double? Longitude,
    bool IsActive);

public sealed record UpdateStoreInventoryRequest(
    Guid ProductId,
    [Range(0, InventoryConstants.MaxQuantity)] int QuantityAvailable,
    [Range(0, InventoryConstants.MaxQuantity)] int LowStockThreshold);

public sealed record CreateProductRequest(
    [Required] Guid CategoryId,
    [Required] Guid BrandId,
    [Required, MaxLength(220)] string Name,
    [Required, MaxLength(80)] string Sku,
    [Required, MaxLength(500)] string ShortDescription,
    [MaxLength(2000)] string? FullDescription,
    [Range(ValidationConstants.PriceMin, ValidationConstants.PriceMax)] decimal Price,
    [Range(ValidationConstants.PriceMin, ValidationConstants.PriceMax)] decimal? WasPrice,
    bool IsFeatured);

public sealed record UpdateAdminProductRequest(
    [Required, MaxLength(220)] string Name,
    [Required, MaxLength(500)] string ShortDescription,
    [MaxLength(2000)] string? FullDescription,
    [Range(ValidationConstants.PriceMin, ValidationConstants.PriceMax)] decimal Price,
    [Range(ValidationConstants.PriceMin, ValidationConstants.PriceMax)] decimal? WasPrice,
    bool IsFeatured,
    bool IsDeleted,
    IReadOnlyCollection<UpdateAdminInventoryRequest> Inventory);

public sealed record UpdateAdminInventoryRequest(
    Guid StoreId,
    [Range(0, InventoryConstants.MaxQuantity)] int QuantityAvailable,
    [Range(0, InventoryConstants.MaxQuantity)] int LowStockThreshold = InventoryConstants.DefaultLowStockThreshold);

public sealed record AdminOrderDto(
    Guid Id,
    string OrderNumber,
    string Status,
    string CustomerName,
    string CustomerEmail,
    int ItemCount,
    decimal Total,
    DateTime PlacedAt,
    DateTime UpdatedAt);

public sealed record UpdateOrderStatusRequest(
    [Required, MaxLength(40)] string Status);

public sealed record AuditLogDto(
    Guid Id,
    Guid ActorUserId,
    string ActorEmail,
    string Action,
    string EntityType,
    string EntityId,
    string Summary,
    DateTime CreatedAtUtc);

public sealed record AdminActor(Guid UserId, string Email);
