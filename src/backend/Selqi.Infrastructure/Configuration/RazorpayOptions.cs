namespace Selqi.Infrastructure.Configuration;

public sealed class RazorpayOptions
{
    public const string SectionName = "Razorpay";

    public string KeyId { get; init; } = string.Empty;
    public string KeySecret { get; init; } = string.Empty;

    // Razorpay sandbox works best with INR; switch to GBP once international
    // payments are enabled on your Razorpay account.
    public string Currency { get; init; } = "INR";

    // GBP → INR conversion rate applied before sending amount to Razorpay.
    // Update this when the exchange rate changes significantly.
    public decimal GbpToInrRate { get; init; } = 107.0m;

    public string CompanyName { get; init; } = "Selqi";
    public string CompanyLogoUrl { get; init; } = string.Empty;
    public string ThemeColor { get; init; } = "#4f46e5";
}
