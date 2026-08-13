using Selqi.Application.Catalog;
using Selqi.Domain.Shopping;

namespace Selqi.Application.Wishlist;

public sealed class WishlistService(IWishlistRepository repo, ICatalogReadRepository catalog) : IWishlistService
{
    public async Task<WishlistDto> GetAsync(Guid userId, CancellationToken ct = default)
    {
        var items = await repo.GetByUserIdAsync(userId, ct);
        return new WishlistDto(items.Select(ToDto).ToList());
    }

    public async Task<WishlistDto> AddAsync(Guid userId, AddToWishlistRequest request, CancellationToken ct = default)
    {
        var product = await catalog.GetProductByIdAsync(request.ProductId, ct)
            ?? throw new KeyNotFoundException("Product not found.");

        var existing = await repo.FindAsync(userId, request.ProductId, ct);
        if (existing is null)
        {
            await repo.AddAsync(new WishlistItem
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProductId = request.ProductId,
                AddedAtUtc = DateTime.UtcNow
            }, ct);
            await repo.SaveChangesAsync(ct);
        }

        return await GetAsync(userId, ct);
    }

    public async Task<WishlistDto> RemoveAsync(Guid userId, Guid productId, CancellationToken ct = default)
    {
        var item = await repo.FindAsync(userId, productId, ct);
        if (item is not null)
        {
            await repo.RemoveAsync(item, ct);
            await repo.SaveChangesAsync(ct);
        }
        return await GetAsync(userId, ct);
    }

    private static WishlistItemDto ToDto(WishlistItem i) => new(
        i.Id, i.ProductId,
        i.Product?.Name ?? string.Empty,
        i.Product?.Slug ?? string.Empty,
        i.Product?.Images.OrderBy(img => img.DisplayOrder).FirstOrDefault()?.Url,
        i.Product?.Price ?? 0m,
        i.Product?.WasPrice,
        i.Product?.Inventory.Sum(inv => inv.QuantityAvailable) > 0,
        i.AddedAtUtc);
}
