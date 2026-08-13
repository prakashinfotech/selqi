using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Selqi.Application.Recommendations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Selqi.Api.Controllers;

[ApiController]
[Route("api/v1/recommendations")]
public sealed class RecommendationsController(IAiRecommendationService recommendationService)
    : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<AiSuggestionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetRecommendations(CancellationToken ct)
    {
        Guid? userId = null;

        if (User.Identity?.IsAuthenticated == true)
        {
            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (Guid.TryParse(sub, out var parsed))
                userId = parsed;
        }

        var suggestions = await recommendationService.GetSuggestionsAsync(userId, ct);
        return Ok(suggestions);
    }
}
