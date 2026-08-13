using System.ComponentModel.DataAnnotations;

namespace Selqi.Application.Orders;

public record CheckoutRequest(
    [Required] DeliveryAddressDto DeliveryAddress,
    [Required] string DeliveryOption,
    [Required] PaymentDto Payment);

public record DeliveryAddressDto(
    [Required, MinLength(2), MaxLength(100)] string FirstName,
    [Required, MinLength(2), MaxLength(100)] string LastName,
    [Required, MinLength(5), MaxLength(250)] string Line1,
    [MaxLength(250)] string? Line2,
    [Required, MinLength(2), MaxLength(120)] string Town,
    [MaxLength(120)] string? County,
    [Required, MaxLength(20),
     RegularExpression(@"^[A-Za-z]{1,2}\d[\dA-Za-z]?\s*\d[A-Za-z]{2}$",
         ErrorMessage = "Enter a valid UK postcode (e.g. SW1A 1AA).")]
    string Postcode,
    [Required, MaxLength(20),
     RegularExpression(@"^[\+\d][\d\s\-\(\)]{6,18}$",
         ErrorMessage = "Enter a valid phone number.")]
    string Phone);

/// <summary>
/// Either provide SavedCardId (fast checkout) OR CardNumber+CardHolder+Expiry (new card).
/// SaveCard=true also persists the new card for future use.
/// </summary>
public record PaymentDto(
    // ── New card fields (required when SavedCardId is null) ────────────────
    [MaxLength(4)] string? CardNumber,
    [MaxLength(100)] string? CardHolder,
    [RegularExpression(@"^(0[1-9]|1[0-2])$")] string? ExpiryMonth,
    [RegularExpression(@"^\d{2}$")] string? ExpiryYear,
    // ── Saved card fast checkout ───────────────────────────────────────────
    Guid? SavedCardId,
    // ── Save new card for future use ──────────────────────────────────────
    bool SaveCard = false,
    [MaxLength(20)] string? CardBrand = null);

public record OrderSummaryDto(
    Guid Id,
    string OrderNumber,
    string Status,
    int ItemCount,
    decimal Total,
    DateTime PlacedAt);

public record OrderStatusHistoryDto(
    string Status,
    DateTime ChangedAt,
    string? Note);

public record OrderDetailDto(
    Guid Id,
    string OrderNumber,
    string Status,
    IReadOnlyList<OrderItemDto> Items,
    decimal SubTotal,
    decimal DeliveryCharge,
    decimal Total,
    string DeliveryName,
    string DeliveryAddress,
    string PaymentReference,
    DateTime PlacedAt,
    IReadOnlyList<OrderStatusHistoryDto> StatusHistory);

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    string ProductSku,
    string? ProductImageUrl,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal);

// ── Razorpay ──────────────────────────────────────────────────────────────────

public record CreateRazorpayOrderRequest(
    [Required] string DeliveryOption);

public record CreateRazorpayOrderResponse(
    string RazorpayOrderId,
    long Amount,
    string Currency,
    string KeyId,
    string CompanyName,
    string CompanyLogoUrl,
    string ThemeColor);

public record VerifyRazorpayPaymentRequest(
    [Required] string RazorpayOrderId,
    [Required] string RazorpayPaymentId,
    [Required] string RazorpaySignature,
    [Required] DeliveryAddressDto DeliveryAddress,
    [Required] string DeliveryOption);
