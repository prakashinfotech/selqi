using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Selqi.Application.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Selqi.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/orders")]
public sealed class OrdersController(IOrderService orderService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

    [HttpGet]
    public async Task<IActionResult> GetOrders(CancellationToken ct)
        => Ok(await orderService.GetOrdersAsync(UserId, ct));

    [HttpGet("{orderNumber}")]
    public async Task<IActionResult> GetOrder(string orderNumber, CancellationToken ct)
    {
        try
        {
            var order = await orderService.GetOrderAsync(UserId, orderNumber, ct);
            return Ok(order);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
