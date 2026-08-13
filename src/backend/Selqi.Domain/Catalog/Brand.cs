namespace Selqi.Domain.Catalog;

public sealed class Brand
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Slug { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public ICollection<Product> Products { get; init; } = [];
}
