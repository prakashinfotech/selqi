using Selqi.Application.Constants;
using System.ComponentModel.DataAnnotations;

namespace Selqi.Application.Auth;

public record RegisterRequest(
    [Required, EmailAddress, MaxLength(ValidationConstants.EmailMaxLength)] string Email,
    [Required, MinLength(ValidationConstants.PasswordMinLength), MaxLength(ValidationConstants.PasswordMaxLength)] string Password,
    [Required, MinLength(ValidationConstants.NameMinLength), MaxLength(ValidationConstants.NameMaxLength)] string FirstName,
    [Required, MinLength(ValidationConstants.NameMinLength), MaxLength(ValidationConstants.NameMaxLength)] string LastName);

public record LoginRequest(
    [Required, EmailAddress, MaxLength(ValidationConstants.EmailMaxLength)] string Email,
    [Required, MinLength(1)] string Password);

public record AuthResponse(
    string Token,
    DateTime ExpiresAt,
    UserDto User);

public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string Role);

public record ForgotPasswordRequest(
    [Required, EmailAddress, MaxLength(ValidationConstants.EmailMaxLength)] string Email);

public record ResetPasswordRequest(
    [Required] string Token,
    [Required, MinLength(ValidationConstants.PasswordMinLength), MaxLength(ValidationConstants.PasswordMaxLength)] string NewPassword,
    [Required, MinLength(ValidationConstants.PasswordMinLength), MaxLength(ValidationConstants.PasswordMaxLength)] string ConfirmPassword);
