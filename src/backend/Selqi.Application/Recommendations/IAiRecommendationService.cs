namespace Selqi.Application.Recommendations;

public interface IAiRecommendationService
{
    Task<IReadOnlyList<AiSuggestionDto>> GetSuggestionsAsync(Guid? userId, CancellationToken ct = default);
}
