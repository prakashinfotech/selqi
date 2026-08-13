namespace Selqi.Domain.Catalog;

public sealed class ProductSpecification
{
    public required Guid Id { get; init; }

    public required Guid ProductId { get; init; }

    public required string Name { get; init; }

    public required string Value { get; init; }

    public int DisplayOrder { get; init; }

    public Product Product { get; init; } = null!;
}
