using Selqi.Application.Basket;
using Selqi.Domain.Shopping;
using Selqi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Selqi.Infrastructure.Shopping;

internal sealed class EfBasketRepository(SelqiDbContext db) : IBasketRepository
{
    public Task<Basket?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => db.Baskets
            .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                    .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(b => b.UserId == userId, ct);

    public async Task<Basket> GetOrCreateAsync(Guid userId, CancellationToken ct = default)
    {
        var basket = await GetByUserIdAsync(userId, ct);
        if (basket is not null) return basket;

        basket = new Basket
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
        db.Baskets.Add(basket);
        await db.SaveChangesAsync(ct);
        return basket;
    }

    public Task<BasketItem?> GetItemAsync(Guid itemId, CancellationToken ct = default)
        => db.BasketItems.FirstOrDefaultAsync(i => i.Id == itemId, ct);

    public Task AddItemAsync(BasketItem item, CancellationToken ct = default)
    {
        db.BasketItems.Add(item);
        return Task.CompletedTask;
    }

    public Task RemoveItemAsync(BasketItem item, CancellationToken ct = default)
    {
        db.BasketItems.Remove(item);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}
