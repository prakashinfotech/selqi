namespace Selqi.Infrastructure.Configuration;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Secret { get; init; } = string.Empty;
    public string Issuer { get; init; } = "SelqiApi";
    public string Audience { get; init; } = "SelqiClient";
    public int ExpiryMinutes { get; init; } = 1440;
}
