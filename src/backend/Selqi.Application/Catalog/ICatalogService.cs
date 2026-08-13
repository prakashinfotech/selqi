namespace Selqi.Application.Catalog;

public interface ICatalogService
{
    Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<BrandDto>> GetBrandsAsync(CancellationToken cancellationToken);

    Task<PagedResultDto<ProductSummaryDto>> GetProductsAsync(
        ProductQuery query,
        CancellationToken cancellationToken);

    Task<ProductDetailDto?> GetProductBySlugAsync(
        string slug,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<ProductAvailabilityDto>> GetProductAvailabilityAsync(
        Guid productId,
        CancellationToken cancellationToken);
}
