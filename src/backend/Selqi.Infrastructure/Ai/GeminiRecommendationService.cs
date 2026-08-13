using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Selqi.Application.Catalog;
using Selqi.Application.Orders;
using Selqi.Application.Recommendations;
using Selqi.Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Selqi.Infrastructure.Ai;

public sealed class GeminiRecommendationService(
    IHttpClientFactory httpClientFactory,
    ICatalogReadRepository catalogRepo,
    IOrderRepository orderRepo,
    IMemoryCache cache,
    IOptions<GeminiOptions> options,
    IOptions<RecommendationOptions> recommendationOptions,
    ILogger<GeminiRecommendationService> logger) : IAiRecommendationService
{
    private static readonly JsonSerializerOptions _jsonOpts =
        new() { PropertyNameCaseInsensitive = true };

    public async Task<IReadOnlyList<AiSuggestionDto>> GetSuggestionsAsync(
        Guid? userId, CancellationToken ct = default)
    {
        var recOpts = recommendationOptions.Value;
        string cacheKey = $"ai-recs:{userId?.ToString() ?? "anon"}";
        if (cache.TryGetValue(cacheKey, out IReadOnlyList<AiSuggestionDto>? cached))
            return cached!;

        // No rating sort — avoids duplicating the "Popular products" section on the home page
        var pool = await catalogRepo.GetProductsAsync(
            new ProductQuery(null, null, null, null, null, true, null, "new", 1, recOpts.ProductPoolSize), ct);

        string userContext = BuildUserContext(userId, await LoadOrderHistoryAsync(userId, recOpts, ct));
        string prompt = BuildPrompt(pool.Items, userContext, recOpts.SuggestionCount);

        IReadOnlyList<AiSuggestionDto> suggestions;
        try
        {
            var rawText = await CallGeminiAsync(prompt, ct);
            logger.LogInformation("Gemini raw response: {Response}", rawText);
            var cleanJson = StripMarkdownFences(rawText);
            suggestions = ParseSuggestions(cleanJson, pool.Items);

            if (suggestions.Count == 0)
                throw new InvalidOperationException("Gemini returned 0 valid product matches.");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Gemini call failed — returning fallback picks.");
            // Fallback: take products from the middle of the pool so they differ from top-rated Popular Products
            suggestions = pool.Items
                .Skip(Math.Max(0, pool.Items.Count / 2))
                .Take(recOpts.SuggestionCount)
                .Select(p => new AiSuggestionDto(p, "You might like this"))
                .ToList();
        }

        cache.Set(cacheKey, suggestions,
            new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(recOpts.CacheDurationMinutes) });

        return suggestions;
    }

    private async Task<IEnumerable<string>> LoadOrderHistoryAsync(Guid? userId, RecommendationOptions recOpts, CancellationToken ct)
    {
        if (!userId.HasValue) return [];

        var orders = await orderRepo.GetByUserIdAsync(userId.Value, ct);
        return orders
            .OrderByDescending(o => o.PlacedAtUtc)
            .Take(recOpts.RecentOrdersCount)
            .SelectMany(o => o.Items)
            .Select(i => i.ProductName)
            .Distinct()
            .Take(recOpts.RecentProductNamesCount);
    }

    private static string BuildUserContext(Guid? userId, IEnumerable<string> purchasedItems)
    {
        var items = purchasedItems.ToList();
        if (!userId.HasValue || items.Count == 0)
            return "This is a new visitor with no purchase history.";

        return $"This customer has previously purchased: {string.Join(", ", items)}.";
    }

    private static string BuildPrompt(IReadOnlyCollection<ProductSummaryDto> products, string userContext, int suggestionCount)
    {
        var productList = products.Select(p => new
        {
            slug = p.Slug,
            name = p.Name,
            category = p.CategoryName,
            brand = p.BrandName,
            price = p.Price,
            rating = p.Rating
        });

        return $$"""
            You are a product recommendation engine for Selqi UK, a retail store.

            User context: {{userContext}}

            Available products:
            {{JsonSerializer.Serialize(productList)}}

            Select exactly {{suggestionCount}} products from the list above that this user would most likely want to buy.
            Rules:
            - Ensure variety across different categories
            - Consider purchase history if provided; suggest complementary or similar items
            - Favour higher-rated products
            - Write each reason as a friendly, short sentence of at most 10 words

            Respond ONLY with a valid JSON array, no markdown fences, no extra text:
            [{"slug":"product-slug","reason":"Short friendly reason"}]
            """;
    }

    private IReadOnlyList<AiSuggestionDto> ParseSuggestions(
        string rawJson, IReadOnlyCollection<ProductSummaryDto> pool)
    {
        var picks = JsonSerializer.Deserialize<List<GeminiPick>>(rawJson, _jsonOpts) ?? [];

        return picks
            .Select(pick => new
            {
                pick,
                product = pool.FirstOrDefault(p => p.Slug == pick.Slug)
            })
            .Where(x => x.product is not null)
            .Select(x => new AiSuggestionDto(x.product!, x.pick.Reason ?? "Recommended for you"))
            .ToList();
    }

    private static string StripMarkdownFences(string text)
    {
        var trimmed = text.Trim();
        if (trimmed.StartsWith("```"))
        {
            var firstNewline = trimmed.IndexOf('\n');
            var lastFence = trimmed.LastIndexOf("```");
            if (firstNewline > 0 && lastFence > firstNewline)
                return trimmed[(firstNewline + 1)..lastFence].Trim();
        }
        return trimmed;
    }

    private async Task<string> CallGeminiAsync(string prompt, CancellationToken ct)
    {
        var geminiOptions = options.Value;
        var client = httpClientFactory.CreateClient("Gemini");

        // responseMimeType omitted intentionally — causes 400 on some key tiers;
        // JSON output is enforced via prompt instructions + StripMarkdownFences instead.
        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt } } }
            }
        };

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/" +
                  $"{geminiOptions.Model}:generateContent?key={geminiOptions.ApiKey}";

        using var response = await client.PostAsJsonAsync(url, requestBody, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);
            logger.LogError("Gemini API error {Status}: {Body}", response.StatusCode, errorBody);
            response.EnsureSuccessStatusCode();
        }

        var result = await response.Content.ReadFromJsonAsync<GeminiResponse>(
            cancellationToken: ct);

        return result?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? "[]";
    }

    private sealed record GeminiPick(string Slug, string? Reason);

    private sealed class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public List<GeminiCandidate>? Candidates { get; set; }
    }

    private sealed class GeminiCandidate
    {
        [JsonPropertyName("content")]
        public GeminiContent? Content { get; set; }
    }

    private sealed class GeminiContent
    {
        [JsonPropertyName("parts")]
        public List<GeminiPart>? Parts { get; set; }
    }

    private sealed class GeminiPart
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
