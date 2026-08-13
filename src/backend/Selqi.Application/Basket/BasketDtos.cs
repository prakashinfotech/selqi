using System.ComponentModel.DataAnnotations;

namespace Selqi.Application.Basket;

public record BasketDto(
    Guid Id,
    IReadOnlyList<BasketItemDto> Items,
    int ItemCount,
    decimal SubTotal);

public record BasketItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string ProductSlug,
    string? ProductImageUrl,
    decimal UnitPrice,
    decimal? WasPrice,
    int Quantity,
    decimal LineTotal);

public record AddToBasketRequest(
    [Required] Guid ProductId,
    [Range(1, 99)] int Quantity = 1);

public record UpdateBasketItemRequest(
    [Range(1, 99)] int Quantity);
