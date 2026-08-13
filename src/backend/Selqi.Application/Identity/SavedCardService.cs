using Selqi.Domain.Identity;

namespace Selqi.Application.Identity;

internal sealed class SavedCardService(ISavedCardRepository repository) : ISavedCardService
{
    public async Task<IReadOnlyList<SavedCardDto>> GetCardsAsync(Guid userId, CancellationToken ct = default)
    {
        var cards = await repository.GetByUserIdAsync(userId, ct);
        return cards.Select(ToDto).ToArray();
    }

    public async Task<SavedCardDto> SaveCardAsync(Guid userId, SaveCardRequest request, CancellationToken ct = default)
    {
        var existing = await repository.GetByUserIdAsync(userId, ct);

        // Clear default flag on others if this is being set as default
        if (request.SetAsDefault)
        {
            foreach (var c in existing.Where(c => c.IsDefault))
                c.IsDefault = false;
        }

        var card = new SavedCard
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CardholderName = request.CardholderName.Trim(),
            Last4 = request.Last4,
            Brand = request.Brand,
            ExpiryMonth = request.ExpiryMonth,
            ExpiryYear = request.ExpiryYear,
            Token = Guid.NewGuid().ToString("N"),
            IsDefault = request.SetAsDefault || !existing.Any(),
            CreatedAtUtc = DateTime.UtcNow
        };

        repository.Add(card);
        await repository.SaveChangesAsync(ct);
        return ToDto(card);
    }

    public async Task DeleteCardAsync(Guid userId, Guid cardId, CancellationToken ct = default)
    {
        var card = await repository.GetByIdAsync(cardId, ct)
            ?? throw new KeyNotFoundException("Saved card not found.");

        if (card.UserId != userId)
            throw new UnauthorizedAccessException("Access denied.");

        var wasDefault = card.IsDefault;
        repository.Remove(card);
        await repository.SaveChangesAsync(ct);

        // Promote oldest remaining card to default if the deleted one was default
        if (wasDefault)
        {
            var remaining = await repository.GetByUserIdAsync(userId, ct);
            var next = remaining.OrderBy(c => c.CreatedAtUtc).FirstOrDefault();
            if (next is not null)
            {
                next.IsDefault = true;
                await repository.SaveChangesAsync(ct);
            }
        }
    }

    public async Task SetDefaultAsync(Guid userId, Guid cardId, CancellationToken ct = default)
    {
        var cards = await repository.GetByUserIdAsync(userId, ct);
        var target = cards.FirstOrDefault(c => c.Id == cardId)
            ?? throw new KeyNotFoundException("Saved card not found.");

        if (target.UserId != userId)
            throw new UnauthorizedAccessException("Access denied.");

        foreach (var c in cards)
            c.IsDefault = c.Id == cardId;

        await repository.SaveChangesAsync(ct);
    }

    private static SavedCardDto ToDto(SavedCard c) => new(
        c.Id, c.CardholderName, c.Last4, c.Brand,
        c.ExpiryMonth, c.ExpiryYear, c.IsDefault, c.CreatedAtUtc);
}
