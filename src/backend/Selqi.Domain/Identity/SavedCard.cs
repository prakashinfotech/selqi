namespace Selqi.Domain.Identity;

public sealed class SavedCard
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string CardholderName { get; set; } = string.Empty;
    public string Last4 { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;   // Visa, Mastercard, Amex, etc.
    public string ExpiryMonth { get; set; } = string.Empty;
    public string ExpiryYear { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;   // mock gateway token (GUID)
    public bool IsDefault { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
