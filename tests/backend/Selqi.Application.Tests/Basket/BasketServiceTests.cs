using Selqi.Application.Basket;
using Selqi.Application.Catalog;
using Selqi.Domain.Catalog;
using Selqi.Domain.Shopping;
using Xunit;

namespace Selqi.Application.Tests.Basket;

public sealed class BasketServiceTests
{
    [Fact]
    public async Task GetAsyncReturnsEmptyBasketWhenUserHasNoBasket()
    {
        var service = new BasketService(new FakeBasketRepository(), new FakeCatalogReadRepository());

        var result = await service.GetAsync(Guid.NewGuid());

        Assert.Equal(Guid.Empty, result.Id);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.ItemCount);
        Assert.Equal(0m, result.SubTotal);
    }

    [Fact]
    public async Task AddItemAsyncCapsExistingItemQuantityAtNinetyNine()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var basket = new Domain.Shopping.Basket
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Items =
            [
                new BasketItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    Quantity = 98,
                    UnitPrice = 20m,
                    Product = CreateProduct(productId)
                }
            ]
        };
        var service = new BasketService(
            new FakeBasketRepository(basket),
            new FakeCatalogReadRepository(productId));

        var result = await service.AddItemAsync(userId, new AddToBasketRequest(productId, 5));

        Assert.Equal(99, result.Items.Single().Quantity);
        Assert.Equal(1980m, result.SubTotal);
    }

    private static Product CreateProduct(Guid id) => new()
    {
        Id = id,
        CategoryId = Guid.NewGuid(),
        BrandId = Guid.NewGuid(),
        Name = "Air fryer",
        Slug = "air-fryer",
        ShortDescription = "Compact air fryer",
        Sku = "SKU-1",
        Price = 20m,
        Rating = 4.5m,
        ReviewCount = 12,
        IsFeatured = true,
        IsDeleted = false,
        CreatedAtUtc = DateTime.UtcNow
    };

    private sealed class FakeBasketRepository(Domain.Shopping.Basket? basket = null) : IBasketRepository
    {
        public Task<Domain.Shopping.Basket?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
            => Task.FromResult(basket);

        public Task<Domain.Shopping.Basket> GetOrCreateAsync(Guid userId, CancellationToken ct = default)
        {
            basket ??= new Domain.Shopping.Basket { Id = Guid.NewGuid(), UserId = userId };
            return Task.FromResult(basket);
        }

        public Task<BasketItem?> GetItemAsync(Guid itemId, CancellationToken ct = default)
            => Task.FromResult(basket?.Items.FirstOrDefault(item => item.Id == itemId));

        public Task AddItemAsync(BasketItem item, CancellationToken ct = default)
        {
            basket?.Items.Add(item);
            return Task.CompletedTask;
        }

        public Task RemoveItemAsync(BasketItem item, CancellationToken ct = default)
        {
            basket?.Items.Remove(item);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
            => Task.CompletedTask;
    }

    private sealed class FakeCatalogReadRepository(Guid? productId = null) : ICatalogReadRepository
    {
        public Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyCollection<CategoryDto>>([]);

        public Task<IReadOnlyCollection<BrandDto>> GetBrandsAsync(CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyCollection<BrandDto>>([]);

        public Task<PagedResultDto<ProductSummaryDto>> GetProductsAsync(ProductQuery query, CancellationToken cancellationToken)
            => Task.FromResult(new PagedResultDto<ProductSummaryDto>([], 1, 24, 0, 0));

        public Task<ProductDetailDto?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken)
            => Task.FromResult<ProductDetailDto?>(null);

        public Task<ProductDetailDto?> GetProductByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (productId is not null && id == productId)
            {
                return Task.FromResult<ProductDetailDto?>(new ProductDetailDto(
                    id,
                    "Air fryer",
                    "air-fryer",
                    "Kitchen",
                    "kitchen",
                    "Selqi",
                    "Compact air fryer",
                    null,
                    "SKU-1",
                    20m,
                    null,
                    4.5m,
                    12,
                    true,
                    [],
                    [],
                    []));
            }

            return Task.FromResult<ProductDetailDto?>(null);
        }

        public Task<IReadOnlyCollection<ProductAvailabilityDto>> GetProductAvailabilityAsync(Guid productId, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyCollection<ProductAvailabilityDto>>([]);
    }
}
