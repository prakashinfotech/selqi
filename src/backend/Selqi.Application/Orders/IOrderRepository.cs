using Selqi.Domain.Orders;

namespace Selqi.Application.Orders;

public interface IOrderRepository
{
    Task<IReadOnlyList<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default);
    Task AddAsync(Order order, CancellationToken ct = default);
    void AddStatusHistory(OrderStatusHistory entry);
    Task SaveChangesAsync(CancellationToken ct = default);
}
