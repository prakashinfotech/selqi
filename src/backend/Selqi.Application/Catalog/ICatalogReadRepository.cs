namespace Selqi.Application.Catalog;

public interface ICatalogReadRepository
{
    Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<BrandDto>> GetBrandsAsync(CancellationToken cancellationToken);

    Task<PagedResultDto<ProductSummaryDto>> GetProductsAsync(
        ProductQuery query,
        CancellationToken cancellationToken);

    Task<ProductDetailDto?> GetProductBySlugAsync(
        string slug,
        CancellationToken cancellationToken);

    Task<ProductDetailDto?> GetProductByIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyCollection<ProductAvailabilityDto>> GetProductAvailabilityAsync(
        Guid productId,
        CancellationToken cancellationToken);
}
