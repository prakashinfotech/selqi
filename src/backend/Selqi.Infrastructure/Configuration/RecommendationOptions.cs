namespace Selqi.Infrastructure.Configuration;

public sealed class RecommendationOptions
{
    public const string SectionName = "Recommendations";

    public int ProductPoolSize { get; set; } = 50;
    public int SuggestionCount { get; set; } = 6;
    public int CacheDurationMinutes { get; set; } = 30;
    public int RecentOrdersCount { get; set; } = 5;
    public int RecentProductNamesCount { get; set; } = 10;
}
