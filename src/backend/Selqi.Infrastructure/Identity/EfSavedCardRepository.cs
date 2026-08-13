using Selqi.Application.Identity;
using Selqi.Domain.Identity;
using Selqi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Selqi.Infrastructure.Identity;

internal sealed class EfSavedCardRepository(SelqiDbContext db) : ISavedCardRepository
{
    public Task<IReadOnlyList<SavedCard>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => db.SavedCards
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.IsDefault)
            .ThenByDescending(c => c.CreatedAtUtc)
            .ToListAsync(ct)
            .ContinueWith<IReadOnlyList<SavedCard>>(t => t.Result, ct);

    public Task<SavedCard?> GetByIdAsync(Guid cardId, CancellationToken ct = default)
        => db.SavedCards.FirstOrDefaultAsync(c => c.Id == cardId, ct);

    public void Add(SavedCard card) => db.SavedCards.Add(card);

    public void Remove(SavedCard card) => db.SavedCards.Remove(card);

    public Task SaveChangesAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
}
