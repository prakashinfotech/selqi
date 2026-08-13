using Selqi.Application.Catalog;
using Selqi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Selqi.Infrastructure.Catalog;

internal sealed class EfCatalogReadRepository(SelqiDbContext dbContext) : ICatalogReadRepository
{
    public async Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var all = await dbContext.Categories
            .AsNoTracking()
            .Where(category => !category.IsDeleted)
            .OrderBy(category => category.DisplayOrder)
            .ToArrayAsync(cancellationToken);

        var subsByParent = all
            .Where(c => c.ParentCategoryId != null)
            .GroupBy(c => c.ParentCategoryId!.Value)
            .ToDictionary(g => g.Key, g => g.ToArray());

        return all
            .Where(c => c.ParentCategoryId == null)
            .Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.Slug,
                c.Description,
                c.ImageUrl,
                c.DisplayOrder,
                null,
                subsByParent.TryGetValue(c.Id, out var subs)
                    ? subs.Select(s => new CategoryDto(s.Id, s.Name, s.Slug, s.Description, s.ImageUrl, s.DisplayOrder, c.Id, []))
                           .ToArray()
                    : []))
            .ToArray();
    }

    public async Task<IReadOnlyCollection<BrandDto>> GetBrandsAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Brands
            .AsNoTracking()
            .OrderBy(brand => brand.Name)
            .Select(brand => new BrandDto(brand.Id, brand.Name, brand.Slug))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<PagedResultDto<ProductSummaryDto>> GetProductsAsync(
        ProductQuery query,
        CancellationToken cancellationToken)
    {
        var products = dbContext.Products
            .AsNoTracking()
            .Where(product => !product.IsDeleted && !product.Category.IsDeleted);

        if (query.Search is not null)
        {
            products = products.Where(product =>
                product.Name.Contains(query.Search) ||
                product.ShortDescription.Contains(query.Search) ||
                product.Brand.Name.Contains(query.Search) ||
                product.Category.Name.Contains(query.Search));
        }

        if (query.CategorySlug is not null)
        {
            products = products.Where(product =>
                product.Category.Slug == query.CategorySlug ||
                (product.Category.ParentCategoryId != null &&
                 product.Category.ParentCategory!.Slug == query.CategorySlug));
        }

        if (query.BrandSlug is not null)
        {
            products = products.Where(product => product.Brand.Slug == query.BrandSlug);
        }

        if (query.MinPrice is not null)
        {
            products = products.Where(product => product.Price >= query.MinPrice);
        }

        if (query.MaxPrice is not null)
        {
            products = products.Where(product => product.Price <= query.MaxPrice);
        }

        if (query.AvailableOnly is true)
        {
            products = products.Where(product => product.Inventory.Any(inventory => inventory.QuantityAvailable > 0));
        }

        if (query.OffersOnly is true)
        {
            products = products.Where(product => product.WasPrice != null && product.WasPrice > product.Price);
        }

        products = query.Sort switch
        {
            ProductSortOptions.PriceDesc => products.OrderByDescending(product => product.Price).ThenBy(product => product.Name),
            ProductSortOptions.PriceAsc  => products.OrderBy(product => product.Price).ThenBy(product => product.Name),
            ProductSortOptions.Rating    => products.OrderByDescending(product => product.Rating).ThenBy(product => product.Name),
            ProductSortOptions.Name      => products.OrderBy(product => product.Name),
            ProductSortOptions.New       => products.OrderByDescending(product => product.CreatedAtUtc).ThenBy(product => product.Name),
            _                            => products.OrderByDescending(product => product.IsFeatured).ThenBy(product => product.Name)
        };

        var totalItems = await products.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

        var items = await products
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(product => new ProductSummaryDto(
                product.Id,
                product.Name,
                product.Slug,
                product.Category.Name,
                product.Category.Slug,
                product.Brand.Name,
                product.Price,
                product.WasPrice,
                product.Rating,
                product.ReviewCount,
                product.Inventory.Any(inventory => inventory.QuantityAvailable > 0),
                product.Images
                    .OrderBy(image => image.DisplayOrder)
                    .Select(image => image.Url)
                    .FirstOrDefault()))
            .ToArrayAsync(cancellationToken);

        return new PagedResultDto<ProductSummaryDto>(
            items,
            query.Page,
            query.PageSize,
            totalItems,
            totalPages);
    }

    public async Task<ProductDetailDto?> GetProductBySlugAsync(
        string slug,
        CancellationToken cancellationToken)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Where(product => !product.IsDeleted && !product.Category.IsDeleted && product.Slug == slug)
            .Select(product => new ProductDetailDto(
                product.Id,
                product.Name,
                product.Slug,
                product.Category.Name,
                product.Category.Slug,
                product.Brand.Name,
                product.ShortDescription,
                product.FullDescription,
                product.Sku,
                product.Price,
                product.WasPrice,
                product.Rating,
                product.ReviewCount,
                product.Inventory.Any(inventory => inventory.QuantityAvailable > 0),
                product.Images
                    .OrderBy(image => image.DisplayOrder)
                    .Select(image => new ProductImageDto(image.Url, image.AltText, image.DisplayOrder))
                    .ToArray(),
                product.Specifications
                    .OrderBy(specification => specification.DisplayOrder)
                    .Select(specification => new ProductSpecificationDto(
                        specification.Name,
                        specification.Value,
                        specification.DisplayOrder))
                    .ToArray(),
                product.Inventory
                    .OrderByDescending(inventory => inventory.QuantityAvailable)
                    .ThenBy(inventory => inventory.Store.Name)
                    .Select(inventory => new ProductAvailabilityDto(
                        inventory.StoreId,
                        inventory.Store.Name,
                        inventory.Store.Town,
                        inventory.Store.Postcode,
                        inventory.QuantityAvailable))
                    .ToArray()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProductDetailDto?> GetProductByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Where(product => !product.IsDeleted && product.Id == id)
            .Select(product => new ProductDetailDto(
                product.Id,
                product.Name,
                product.Slug,
                product.Category.Name,
                product.Category.Slug,
                product.Brand.Name,
                product.ShortDescription,
                product.FullDescription,
                product.Sku,
                product.Price,
                product.WasPrice,
                product.Rating,
                product.ReviewCount,
                product.Inventory.Any(inventory => inventory.QuantityAvailable > 0),
                product.Images
                    .OrderBy(image => image.DisplayOrder)
                    .Select(image => new ProductImageDto(image.Url, image.AltText, image.DisplayOrder))
                    .ToArray(),
                product.Specifications
                    .OrderBy(specification => specification.DisplayOrder)
                    .Select(specification => new ProductSpecificationDto(
                        specification.Name, specification.Value, specification.DisplayOrder))
                    .ToArray(),
                product.Inventory
                    .Select(inventory => new ProductAvailabilityDto(
                        inventory.StoreId, inventory.Store.Name, inventory.Store.Town,
                        inventory.Store.Postcode, inventory.QuantityAvailable))
                    .ToArray()))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyCollection<ProductAvailabilityDto>> GetProductAvailabilityAsync(
        Guid productId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Inventory
            .AsNoTracking()
            .Where(inventory => inventory.ProductId == productId)
            .OrderByDescending(inventory => inventory.QuantityAvailable)
            .ThenBy(inventory => inventory.Store.Name)
            .Select(inventory => new ProductAvailabilityDto(
                inventory.StoreId,
                inventory.Store.Name,
                inventory.Store.Town,
                inventory.Store.Postcode,
                inventory.QuantityAvailable))
            .ToArrayAsync(cancellationToken);
    }
}
