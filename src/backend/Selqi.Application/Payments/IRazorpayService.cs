namespace Selqi.Application.Payments;

public interface IRazorpayService
{
    /// <summary>Creates a Razorpay order and returns its ID and amount in smallest currency unit.</summary>
    Task<RazorpayOrderResult> CreateOrderAsync(decimal amount, string receipt, CancellationToken ct = default);

    /// <summary>Verifies the HMAC-SHA256 signature returned by the Razorpay JS SDK after payment.</summary>
    bool VerifySignature(string razorpayOrderId, string razorpayPaymentId, string razorpaySignature);
}

public sealed record RazorpayOrderResult(
    string RazorpayOrderId,
    long AmountInSmallestUnit,
    string Currency,
    string KeyId,
    string CompanyName,
    string CompanyLogoUrl,
    string ThemeColor);
