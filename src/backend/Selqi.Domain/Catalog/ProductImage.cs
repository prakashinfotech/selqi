namespace Selqi.Domain.Catalog;

public sealed class ProductImage
{
    public required Guid Id { get; init; }

    public required Guid ProductId { get; init; }

    public required string Url { get; init; }

    public required string AltText { get; init; }

    public int DisplayOrder { get; init; }

    public Product Product { get; init; } = null!;
}
