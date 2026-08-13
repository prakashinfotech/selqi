using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Selqi.Application.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Selqi.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/saved-cards")]
public sealed class SavedCardsController(ISavedCardService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SavedCardDto>), 200)]
    public async Task<IActionResult> GetCards(CancellationToken ct)
        => Ok(await service.GetCardsAsync(CurrentUserId, ct));

    [HttpPost]
    [ProducesResponseType(typeof(SavedCardDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> SaveCard(SaveCardRequest request, CancellationToken ct)
    {
        try
        {
            var card = await service.SaveCardAsync(CurrentUserId, request, ct);
            return StatusCode(201, card);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{cardId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCard(Guid cardId, CancellationToken ct)
    {
        try
        {
            await service.DeleteCardAsync(CurrentUserId, cardId, ct);
            return NoContent();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }

    [HttpPut("{cardId:guid}/default")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> SetDefault(Guid cardId, CancellationToken ct)
    {
        try
        {
            await service.SetDefaultAsync(CurrentUserId, cardId, ct);
            return NoContent();
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }
}
