using Selqi.Domain.Identity;

namespace Selqi.Application.Identity;

public interface ISavedCardRepository
{
    Task<IReadOnlyList<SavedCard>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<SavedCard?> GetByIdAsync(Guid cardId, CancellationToken ct = default);
    void Add(SavedCard card);
    void Remove(SavedCard card);
    Task SaveChangesAsync(CancellationToken ct = default);
}
