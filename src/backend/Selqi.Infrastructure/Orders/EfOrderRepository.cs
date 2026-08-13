using Selqi.Application.Orders;
using Selqi.Domain.Orders;
using Selqi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Selqi.Infrastructure.Orders;

internal sealed class EfOrderRepository(SelqiDbContext db) : IOrderRepository
{
    public async Task<IReadOnlyList<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await db.Orders
            .Include(o => o.Items)
            .Include(o => o.StatusHistory)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.PlacedAtUtc)
            .ToListAsync(ct);

    public Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default)
        => db.Orders
            .Include(o => o.Items)
            .Include(o => o.StatusHistory)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, ct);

    public Task AddAsync(Order order, CancellationToken ct = default)
    {
        db.Orders.Add(order);
        return Task.CompletedTask;
    }

    public void AddStatusHistory(OrderStatusHistory entry)
        => db.OrderStatusHistories.Add(entry);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}
