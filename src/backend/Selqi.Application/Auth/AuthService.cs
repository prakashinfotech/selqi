using System.Security.Cryptography;
using Selqi.Application.Email;
using Selqi.Domain.Identity;

namespace Selqi.Application.Auth;

public sealed class AuthService(
    IUserRepository users,
    ITokenService tokens,
    IPasswordResetRepository resetTokens,
    IEmailService emailService) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        if (await users.EmailExistsAsync(email, ct))
            throw new InvalidOperationException("An account with this email already exists.");

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = PasswordHasher.Hash(request.Password),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Role = UserRoles.Customer,
            CreatedAtUtc = DateTime.UtcNow
        };

        await users.AddAsync(user, ct);
        await users.SaveChangesAsync(ct);

        var (token, expiresAt) = tokens.Generate(user);
        return new AuthResponse(token, expiresAt, ToDto(user));
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await users.FindByEmailAsync(email, ct)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        user.LastLoginAtUtc = DateTime.UtcNow;
        await users.UpdateAsync(user, ct);
        await users.SaveChangesAsync(ct);

        var (token, expiresAt) = tokens.Generate(user);
        return new AuthResponse(token, expiresAt, ToDto(user));
    }

    public async Task<UserDto> GetCurrentUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await users.FindByIdAsync(userId, ct)
            ?? throw new KeyNotFoundException("User not found.");
        return ToDto(user);
    }

    public async Task ForgotPasswordAsync(
        ForgotPasswordRequest request,
        string frontendBaseUrl,
        CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await users.FindByEmailAsync(email, ct);

        // Always return silently even when email is not found — prevents user enumeration
        if (user is null) return;

        var tokenValue = Convert.ToHexString(RandomNumberGenerator.GetBytes(32)).ToLowerInvariant();

        await resetTokens.AddAsync(new PasswordResetToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = tokenValue,
            ExpiresAtUtc = DateTime.UtcNow.AddHours(1),
            CreatedAtUtc = DateTime.UtcNow
        }, ct);

        await resetTokens.SaveChangesAsync(ct);

        var resetUrl = $"{frontendBaseUrl.TrimEnd('/')}/reset-password?token={tokenValue}";
        await emailService.SendAsync(
            user.Email,
            "Reset your Selqi password",
            EmailTemplates.PasswordReset(user.FirstName, resetUrl),
            ct);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default)
    {
        if (request.NewPassword != request.ConfirmPassword)
            throw new InvalidOperationException("Passwords do not match.");

        var resetToken = await resetTokens.FindByTokenAsync(request.Token, ct)
            ?? throw new InvalidOperationException("Invalid or expired reset link. Please request a new one.");

        if (resetToken.ExpiresAtUtc < DateTime.UtcNow)
            throw new InvalidOperationException("This reset link has expired. Please request a new one.");

        if (resetToken.UsedAtUtc is not null)
            throw new InvalidOperationException("This reset link has already been used.");

        var user = await users.FindByIdAsync(resetToken.UserId, ct)
            ?? throw new InvalidOperationException("User not found.");

        user.PasswordHash = PasswordHasher.Hash(request.NewPassword);
        resetToken.UsedAtUtc = DateTime.UtcNow;

        await users.UpdateAsync(user, ct);
        await resetTokens.SaveChangesAsync(ct);
    }

    private static UserDto ToDto(ApplicationUser u) =>
        new(u.Id, u.Email, u.FirstName, u.LastName, u.Role);
}
