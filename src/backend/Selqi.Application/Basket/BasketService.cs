using Selqi.Application.Catalog;
using Selqi.Domain.Shopping;

namespace Selqi.Application.Basket;

public sealed class BasketService(IBasketRepository basketRepo, ICatalogReadRepository catalogRepo) : IBasketService
{
    public async Task<BasketDto> GetAsync(Guid userId, CancellationToken ct = default)
    {
        var basket = await basketRepo.GetByUserIdAsync(userId, ct);
        return basket is null ? EmptyBasket() : ToDto(basket);
    }

    public async Task<BasketDto> AddItemAsync(Guid userId, AddToBasketRequest request, CancellationToken ct = default)
    {
        var product = await catalogRepo.GetProductByIdAsync(request.ProductId, ct)
            ?? throw new KeyNotFoundException($"Product {request.ProductId} not found.");

        var basket = await basketRepo.GetOrCreateAsync(userId, ct);

        var existing = basket.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (existing is not null)
        {
            existing.Quantity = Math.Min(existing.Quantity + request.Quantity, 99);
        }
        else
        {
            await basketRepo.AddItemAsync(new BasketItem
            {
                Id = Guid.NewGuid(),
                BasketId = basket.Id,
                ProductId = product.Id,
                Quantity = request.Quantity,
                UnitPrice = product.Price,
                AddedAtUtc = DateTime.UtcNow
            }, ct);
        }

        basket.UpdatedAtUtc = DateTime.UtcNow;
        await basketRepo.SaveChangesAsync(ct);
        return ToDto(await ReloadBasketAsync(userId, ct));
    }

    public async Task<BasketDto> UpdateItemAsync(Guid userId, Guid itemId, UpdateBasketItemRequest request, CancellationToken ct = default)
    {
        var item = await basketRepo.GetItemAsync(itemId, ct)
            ?? throw new KeyNotFoundException("Basket item not found.");

        item.Quantity = request.Quantity;
        await basketRepo.SaveChangesAsync(ct);
        return ToDto(await ReloadBasketAsync(userId, ct));
    }

    public async Task<BasketDto> RemoveItemAsync(Guid userId, Guid itemId, CancellationToken ct = default)
    {
        var item = await basketRepo.GetItemAsync(itemId, ct)
            ?? throw new KeyNotFoundException("Basket item not found.");

        await basketRepo.RemoveItemAsync(item, ct);
        await basketRepo.SaveChangesAsync(ct);

        var basket = await basketRepo.GetByUserIdAsync(userId, ct);
        return basket is null ? EmptyBasket() : ToDto(basket);
    }

    public async Task ClearAsync(Guid userId, CancellationToken ct = default)
    {
        var basket = await basketRepo.GetByUserIdAsync(userId, ct);
        if (basket is null) return;

        foreach (var item in basket.Items.ToList())
            await basketRepo.RemoveItemAsync(item, ct);

        await basketRepo.SaveChangesAsync(ct);
    }

    private static BasketDto ToDto(Domain.Shopping.Basket basket)
    {
        var items = basket.Items.Select(i => new BasketItemDto(
            i.Id, i.ProductId,
            i.Product?.Name ?? string.Empty,
            i.Product?.Slug ?? string.Empty,
            i.Product?.Images.OrderBy(img => img.DisplayOrder).FirstOrDefault()?.Url,
            i.UnitPrice,
            i.Product?.WasPrice,
            i.Quantity,
            i.UnitPrice * i.Quantity)).ToList();

        return new BasketDto(basket.Id, items,
            items.Sum(i => i.Quantity),
            items.Sum(i => i.LineTotal));
    }

    private static BasketDto EmptyBasket() =>
        new(Guid.Empty, [], 0, 0m);

    private async Task<Domain.Shopping.Basket> ReloadBasketAsync(Guid userId, CancellationToken ct)
    {
        return await basketRepo.GetByUserIdAsync(userId, ct)
            ?? throw new InvalidOperationException("Basket could not be loaded after saving changes.");
    }
}
