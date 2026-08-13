namespace Selqi.Application.Wishlist;

public interface IWishlistService
{
    Task<WishlistDto> GetAsync(Guid userId, CancellationToken ct = default);
    Task<WishlistDto> AddAsync(Guid userId, AddToWishlistRequest request, CancellationToken ct = default);
    Task<WishlistDto> RemoveAsync(Guid userId, Guid productId, CancellationToken ct = default);
}
