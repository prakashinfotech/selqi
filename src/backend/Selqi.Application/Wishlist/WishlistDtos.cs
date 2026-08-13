using System.ComponentModel.DataAnnotations;

namespace Selqi.Application.Wishlist;

public record WishlistDto(IReadOnlyList<WishlistItemDto> Items);

public record WishlistItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string ProductSlug,
    string? ProductImageUrl,
    decimal Price,
    decimal? WasPrice,
    bool IsAvailable,
    DateTime AddedAt);

public record AddToWishlistRequest([Required] Guid ProductId);
