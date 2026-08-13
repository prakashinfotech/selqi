namespace Selqi.Application.Basket;

public interface IBasketService
{
    Task<BasketDto> GetAsync(Guid userId, CancellationToken ct = default);
    Task<BasketDto> AddItemAsync(Guid userId, AddToBasketRequest request, CancellationToken ct = default);
    Task<BasketDto> UpdateItemAsync(Guid userId, Guid itemId, UpdateBasketItemRequest request, CancellationToken ct = default);
    Task<BasketDto> RemoveItemAsync(Guid userId, Guid itemId, CancellationToken ct = default);
    Task ClearAsync(Guid userId, CancellationToken ct = default);
}
