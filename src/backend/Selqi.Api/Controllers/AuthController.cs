using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Selqi.Application.Auth;
using Selqi.Infrastructure.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Selqi.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController(
    IAuthService authService,
    IOptions<EmailOptions> emailOptions) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken ct)
    {
        try
        {
            var response = await authService.RegisterAsync(request, ct);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
    {
        try
        {
            var response = await authService.LoginAsync(request, ct);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var user = await authService.GetCurrentUserAsync(userId, ct);
        return Ok(user);
    }

    /// <summary>
    /// Sends a password reset link to the supplied email address.
    /// Always returns 200 regardless of whether the email exists (prevents enumeration).
    /// </summary>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request, CancellationToken ct)
    {
        var frontendUrl = emailOptions.Value.FrontendUrl;
        await authService.ForgotPasswordAsync(request, frontendUrl, ct);
        return Ok(new { message = "If an account with that email exists, a reset link has been sent." });
    }

    /// <summary>
    /// Validates the reset token and updates the user's password.
    /// </summary>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken ct)
    {
        try
        {
            await authService.ResetPasswordAsync(request, ct);
            return Ok(new { message = "Password updated successfully. You can now sign in." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
