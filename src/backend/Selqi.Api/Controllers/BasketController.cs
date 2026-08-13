using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Selqi.Application.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Selqi.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/basket")]
public sealed class BasketController(IBasketService basketService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
        => Ok(await basketService.GetAsync(UserId, ct));

    [HttpPost("items")]
    public async Task<IActionResult> AddItem(AddToBasketRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await basketService.AddItemAsync(UserId, request, ct));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("items/{itemId:guid}")]
    public async Task<IActionResult> UpdateItem(Guid itemId, UpdateBasketItemRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await basketService.UpdateItemAsync(UserId, itemId, request, ct));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("items/{itemId:guid}")]
    public async Task<IActionResult> RemoveItem(Guid itemId, CancellationToken ct)
    {
        try
        {
            return Ok(await basketService.RemoveItemAsync(UserId, itemId, ct));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Clear(CancellationToken ct)
    {
        await basketService.ClearAsync(UserId, ct);
        return NoContent();
    }
}
