using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Selqi.Application.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Selqi.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/checkout")]
public sealed class CheckoutController(IOrderService orderService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(CheckoutRequest request, CancellationToken ct)
    {
        try
        {
            var order = await orderService.PlaceOrderAsync(UserId, request, ct);
            return CreatedAtAction(nameof(OrdersController.GetOrder),
                "Orders", new { orderNumber = order.OrderNumber }, order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
