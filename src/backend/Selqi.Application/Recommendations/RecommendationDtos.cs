using Selqi.Application.Catalog;

namespace Selqi.Application.Recommendations;

public sealed record AiSuggestionDto(ProductSummaryDto Product, string Reason);
