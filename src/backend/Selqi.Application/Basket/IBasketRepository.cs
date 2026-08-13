using Selqi.Domain.Shopping;

namespace Selqi.Application.Basket;

public interface IBasketRepository
{
    Task<Domain.Shopping.Basket?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Domain.Shopping.Basket> GetOrCreateAsync(Guid userId, CancellationToken ct = default);
    Task<BasketItem?> GetItemAsync(Guid itemId, CancellationToken ct = default);
    Task AddItemAsync(BasketItem item, CancellationToken ct = default);
    Task RemoveItemAsync(BasketItem item, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
