using Selqi.Domain.Shopping;

namespace Selqi.Application.Wishlist;

public interface IWishlistRepository
{
    Task<IReadOnlyList<WishlistItem>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<WishlistItem?> FindAsync(Guid userId, Guid productId, CancellationToken ct = default);
    Task AddAsync(WishlistItem item, CancellationToken ct = default);
    Task RemoveAsync(WishlistItem item, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
