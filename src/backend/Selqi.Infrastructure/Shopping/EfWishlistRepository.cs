using Selqi.Application.Wishlist;
using Selqi.Domain.Shopping;
using Selqi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Selqi.Infrastructure.Shopping;

internal sealed class EfWishlistRepository(SelqiDbContext db) : IWishlistRepository
{
    public async Task<IReadOnlyList<WishlistItem>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await db.WishlistItems
            .Include(w => w.Product)
                .ThenInclude(p => p.Images)
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.AddedAtUtc)
            .ToListAsync(ct);

    public Task<WishlistItem?> FindAsync(Guid userId, Guid productId, CancellationToken ct = default)
        => db.WishlistItems
            .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId, ct);

    public Task AddAsync(WishlistItem item, CancellationToken ct = default)
    {
        db.WishlistItems.Add(item);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(WishlistItem item, CancellationToken ct = default)
    {
        db.WishlistItems.Remove(item);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}
