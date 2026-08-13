using Selqi.Application.Constants;

namespace Selqi.Application.Catalog;

internal sealed class CatalogService(ICatalogReadRepository catalogReadRepository) : ICatalogService
{

    public Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        return catalogReadRepository.GetCategoriesAsync(cancellationToken);
    }

    public Task<IReadOnlyCollection<BrandDto>> GetBrandsAsync(CancellationToken cancellationToken)
    {
        return catalogReadRepository.GetBrandsAsync(cancellationToken);
    }

    public Task<PagedResultDto<ProductSummaryDto>> GetProductsAsync(
        ProductQuery query,
        CancellationToken cancellationToken)
    {
        var normalizedQuery = query with
        {
            Search = NormalizeString(query.Search),
            CategorySlug = NormalizeString(query.CategorySlug),
            BrandSlug = NormalizeString(query.BrandSlug),
            Sort = NormalizeString(query.Sort),
            Page = query.Page <= 0 ? PaginationConstants.DefaultPage : query.Page,
            PageSize = query.PageSize <= 0 ? PaginationConstants.DefaultPageSize : Math.Min(query.PageSize, PaginationConstants.MaxPageSize)
        };

        return catalogReadRepository.GetProductsAsync(normalizedQuery, cancellationToken);
    }

    public Task<ProductDetailDto?> GetProductBySlugAsync(
        string slug,
        CancellationToken cancellationToken)
    {
        return catalogReadRepository.GetProductBySlugAsync(slug.Trim().ToLowerInvariant(), cancellationToken);
    }

    public Task<IReadOnlyCollection<ProductAvailabilityDto>> GetProductAvailabilityAsync(
        Guid productId,
        CancellationToken cancellationToken)
    {
        return catalogReadRepository.GetProductAvailabilityAsync(productId, cancellationToken);
    }

    private static string? NormalizeString(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLowerInvariant();
    }
}
