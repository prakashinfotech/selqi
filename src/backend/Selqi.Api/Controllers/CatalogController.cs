using Selqi.Application.Catalog;
using Selqi.Application.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Selqi.Api.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class CatalogController(ICatalogService catalogService) : ControllerBase
{
    [HttpGet("categories")]
    [ProducesResponseType<IReadOnlyCollection<CategoryDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await catalogService.GetCategoriesAsync(cancellationToken);

        return Ok(categories);
    }

    [HttpGet("brands")]
    [ProducesResponseType<IReadOnlyCollection<BrandDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBrands(CancellationToken cancellationToken)
    {
        var brands = await catalogService.GetBrandsAsync(cancellationToken);

        return Ok(brands);
    }

    [HttpGet("products")]
    [ProducesResponseType<PagedResultDto<ProductSummaryDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts(
        [FromQuery] string? search,
        [FromQuery] string? category,
        [FromQuery] string? brand,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] bool? availableOnly,
        [FromQuery] bool? offersOnly,
        [FromQuery] string? sort,
        [FromQuery] int page = PaginationConstants.DefaultPage,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        var products = await catalogService.GetProductsAsync(
            new ProductQuery(search, category, brand, minPrice, maxPrice, availableOnly, offersOnly, sort, page, pageSize),
            cancellationToken);

        return Ok(products);
    }

    [HttpGet("products/{slug}")]
    [ProducesResponseType<ProductDetailDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductBySlug(
        string slug,
        CancellationToken cancellationToken)
    {
        var product = await catalogService.GetProductBySlugAsync(slug, cancellationToken);

        return product is null ? NotFound() : Ok(product);
    }

    [HttpGet("products/{id:guid}/availability")]
    [ProducesResponseType<IReadOnlyCollection<ProductAvailabilityDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductAvailability(
        Guid id,
        CancellationToken cancellationToken)
    {
        var availability = await catalogService.GetProductAvailabilityAsync(id, cancellationToken);

        return Ok(availability);
    }
}
