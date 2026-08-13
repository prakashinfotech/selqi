using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Selqi.Application.Wishlist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Selqi.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/wishlist")]
public sealed class WishlistController(IWishlistService wishlistService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
        => Ok(await wishlistService.GetAsync(UserId, ct));

    [HttpPost("items")]
    public async Task<IActionResult> Add(AddToWishlistRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await wishlistService.AddAsync(UserId, request, ct));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("items/{productId:guid}")]
    public async Task<IActionResult> Remove(Guid productId, CancellationToken ct)
        => Ok(await wishlistService.RemoveAsync(UserId, productId, ct));
}
