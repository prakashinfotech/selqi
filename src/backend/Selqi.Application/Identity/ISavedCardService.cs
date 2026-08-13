namespace Selqi.Application.Identity;

public interface ISavedCardService
{
    Task<IReadOnlyList<SavedCardDto>> GetCardsAsync(Guid userId, CancellationToken ct = default);
    Task<SavedCardDto> SaveCardAsync(Guid userId, SaveCardRequest request, CancellationToken ct = default);
    Task DeleteCardAsync(Guid userId, Guid cardId, CancellationToken ct = default);
    Task SetDefaultAsync(Guid userId, Guid cardId, CancellationToken ct = default);
}
