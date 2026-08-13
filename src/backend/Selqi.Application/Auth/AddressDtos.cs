using System.ComponentModel.DataAnnotations;

namespace Selqi.Application.Auth;

public record AddressDto(
    Guid Id,
    string Label,
    string FirstName,
    string LastName,
    string Line1,
    string? Line2,
    string Town,
    string? County,
    string Postcode,
    string Country,
    string Phone,
    bool IsDefault);

public record SaveAddressRequest(
    [Required, MaxLength(100)] string Label,
    [Required, MaxLength(100)] string FirstName,
    [Required, MaxLength(100)] string LastName,
    [Required, MaxLength(250)] string Line1,
    [Required, MaxLength(120)] string Town,
    [Required, MaxLength(20),
     RegularExpression(@"^[A-Za-z]{1,2}\d[\dA-Za-z]?\s*\d[A-Za-z]{2}$",
         ErrorMessage = "Enter a valid UK postcode (e.g. SW1A 1AA).")]
    string Postcode,
    [MaxLength(250)] string? Line2 = null,
    [MaxLength(120)] string? County = null,
    [MaxLength(100)] string Country = "United Kingdom",
    [MaxLength(20),
     RegularExpression(@"^[\+\d][\d\s\-\(\)]{6,18}$",
         ErrorMessage = "Enter a valid phone number.")]
    string? Phone = null,
    bool IsDefault = false);

public record UpdateAddressRequest(
    [Required, MaxLength(100)] string Label,
    [Required, MaxLength(100)] string FirstName,
    [Required, MaxLength(100)] string LastName,
    [Required, MaxLength(250)] string Line1,
    [Required, MaxLength(120)] string Town,
    [Required, MaxLength(20),
     RegularExpression(@"^[A-Za-z]{1,2}\d[\dA-Za-z]?\s*\d[A-Za-z]{2}$",
         ErrorMessage = "Enter a valid UK postcode (e.g. SW1A 1AA).")]
    string Postcode,
    [MaxLength(250)] string? Line2 = null,
    [MaxLength(120)] string? County = null,
    [MaxLength(100)] string Country = "United Kingdom",
    [MaxLength(20),
     RegularExpression(@"^[\+\d][\d\s\-\(\)]{6,18}$",
         ErrorMessage = "Enter a valid phone number.")]
    string? Phone = null,
    bool IsDefault = false);
