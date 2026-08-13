using System.ComponentModel.DataAnnotations;

namespace Selqi.Application.Identity;

public sealed record SavedCardDto(
    Guid Id,
    string CardholderName,
    string Last4,
    string Brand,
    string ExpiryMonth,
    string ExpiryYear,
    bool IsDefault,
    DateTime CreatedAtUtc);

public sealed record SaveCardRequest(
    [Required, MaxLength(100)] string CardholderName,
    [Required, MaxLength(4), MinLength(4)] string Last4,
    [Required, MaxLength(20)] string Brand,
    [Required, RegularExpression(@"^(0[1-9]|1[0-2])$")] string ExpiryMonth,
    [Required, RegularExpression(@"^\d{2}$")] string ExpiryYear,
    bool SetAsDefault);
