using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Selqi.Application.Payments;
using Selqi.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace Selqi.Infrastructure.Payments;

internal sealed class RazorpayService(
    IHttpClientFactory httpClientFactory,
    IOptions<RazorpayOptions> options) : IRazorpayService
{
    private const string BaseUrl = "https://api.razorpay.com";
    private readonly RazorpayOptions _opts = options.Value;

    public async Task<RazorpayOrderResult> CreateOrderAsync(
        decimal amount, string receipt, CancellationToken ct = default)
    {
        // Convert GBP → INR, then to paise (smallest INR unit)
        var amountInInr = amount * _opts.GbpToInrRate;
        var amountInSmallestUnit = (long)Math.Round(amountInInr * 100, MidpointRounding.AwayFromZero);

        var client = CreateClient();

        var body = new
        {
            amount = amountInSmallestUnit,
            currency = _opts.Currency,
            receipt,
            notes = new { description = "Selqi Ecommerce Order" }
        };

        var response = await client.PostAsync(
            "/v1/orders",
            new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"),
            ct);

        response.EnsureSuccessStatusCode();

        using var doc = await JsonDocument.ParseAsync(
            await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);

        var razorpayOrderId = doc.RootElement.GetProperty("id").GetString()
            ?? throw new InvalidOperationException("Razorpay did not return an order ID.");

        return new RazorpayOrderResult(
            razorpayOrderId,
            amountInSmallestUnit,
            _opts.Currency,
            _opts.KeyId,
            _opts.CompanyName,
            _opts.CompanyLogoUrl,
            _opts.ThemeColor);
    }

    public bool VerifySignature(
        string razorpayOrderId, string razorpayPaymentId, string razorpaySignature)
    {
        var message = $"{razorpayOrderId}|{razorpayPaymentId}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_opts.KeySecret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
        var expectedSignature = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(expectedSignature),
            Encoding.UTF8.GetBytes(razorpaySignature));
    }

    private HttpClient CreateClient()
    {
        var client = httpClientFactory.CreateClient("Razorpay");
        client.BaseAddress = new Uri(BaseUrl);
        var credentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{_opts.KeyId}:{_opts.KeySecret}"));
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", credentials);
        return client;
    }
}
