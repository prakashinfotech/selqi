using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Selqi.Application.Basket;
using Selqi.Application.Orders;
using Selqi.Application.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Selqi.Api.Controllers;

[ApiController]
[Route("api/v1/razorpay")]
[Authorize]
public sealed class RazorpayController(
    IRazorpayService razorpayService,
    IOrderService orderService,
    IBasketService basketService) : ControllerBase
{
    /// <summary>
    /// Creates a Razorpay order for the current user's basket.
    /// Call this before opening the Razorpay checkout modal on the frontend.
    /// </summary>
    [HttpPost("create-order")]
    [ProducesResponseType<CreateRazorpayOrderResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateRazorpayOrderRequest request,
        CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var basket = await basketService.GetAsync(userId, ct);

        if (basket.Items.Count == 0)
            return BadRequest(new { message = "Your basket is empty." });

        var subTotal = basket.SubTotal;
        var deliveryCharge = request.DeliveryOption switch
        {
            "express" => 5.95m,
            "standard" => subTotal >= 50m ? 0m : 3.95m,
            _ => 0m
        };

        var total = subTotal + deliveryCharge;
        var receipt = $"selqi_{userId.ToString("N")[..8]}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

        var result = await razorpayService.CreateOrderAsync(total, receipt, ct);

        return Ok(new CreateRazorpayOrderResponse(
            result.RazorpayOrderId,
            result.AmountInSmallestUnit,
            result.Currency,
            result.KeyId,
            result.CompanyName,
            result.CompanyLogoUrl,
            result.ThemeColor));
    }

    /// <summary>
    /// Verifies the Razorpay signature returned by the JS SDK and places the internal order.
    /// Must be called after a successful payment in the Razorpay modal.
    /// </summary>
    [HttpPost("verify-payment")]
    [ProducesResponseType<OrderDetailDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyPayment(
        [FromBody] VerifyRazorpayPaymentRequest request,
        CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        var signatureValid = razorpayService.VerifySignature(
            request.RazorpayOrderId,
            request.RazorpayPaymentId,
            request.RazorpaySignature);

        if (!signatureValid)
            return BadRequest(new { message = "Payment verification failed. Signature mismatch." });

        var order = await orderService.PlaceRazorpayOrderAsync(
            userId, request, request.RazorpayPaymentId, ct);

        return CreatedAtAction(nameof(VerifyPayment), new { orderNumber = order.OrderNumber }, order);
    }
}
