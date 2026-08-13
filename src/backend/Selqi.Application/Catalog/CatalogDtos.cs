namespace Selqi.Application.Catalog;

public sealed record CategoryDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    string? ImageUrl,
    int DisplayOrder,
    Guid? ParentCategoryId,
    IReadOnlyCollection<CategoryDto> SubCategories);

public sealed record BrandDto(
    Guid Id,
    string Name,
    string Slug);

public sealed record ProductSummaryDto(
    Guid Id,
    string Name,
    string Slug,
    string CategoryName,
    string CategorySlug,
    string BrandName,
    decimal Price,
    decimal? WasPrice,
    decimal Rating,
    int ReviewCount,
    bool IsAvailable,
    string? PrimaryImageUrl);

public sealed record ProductDetailDto(
    Guid Id,
    string Name,
    string Slug,
    string CategoryName,
    string CategorySlug,
    string BrandName,
    string ShortDescription,
    string? FullDescription,
    string Sku,
    decimal Price,
    decimal? WasPrice,
    decimal Rating,
    int ReviewCount,
    bool IsAvailable,
    IReadOnlyCollection<ProductImageDto> Images,
    IReadOnlyCollection<ProductSpecificationDto> Specifications,
    IReadOnlyCollection<ProductAvailabilityDto> Availability);

public sealed record ProductImageDto(
    string Url,
    string AltText,
    int DisplayOrder);

public sealed record ProductSpecificationDto(
    string Name,
    string Value,
    int DisplayOrder);

public sealed record ProductAvailabilityDto(
    Guid StoreId,
    string StoreName,
    string Town,
    string Postcode,
    int QuantityAvailable);

public sealed record PagedResultDto<T>(
    IReadOnlyCollection<T> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages);

public sealed record ProductQuery(
    string? Search,
    string? CategorySlug,
    string? BrandSlug,
    decimal? MinPrice,
    decimal? MaxPrice,
    bool? AvailableOnly,
    bool? OffersOnly,
    string? Sort,
    int Page,
    int PageSize);
