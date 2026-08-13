namespace Selqi.Infrastructure.Configuration;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    public required string Provider { get; init; }

    public required string ConnectionStringName { get; init; }
}
