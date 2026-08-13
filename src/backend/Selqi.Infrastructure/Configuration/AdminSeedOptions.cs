namespace Selqi.Infrastructure.Configuration;

public sealed class AdminSeedOptions
{
    public const string SectionName = "AdminSeed";

    public bool Enabled { get; init; }

    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string FirstName { get; init; } = "Admin";

    public string LastName { get; init; } = "User";
}
