namespace Selqi.Domain.Catalog;

public sealed class Product
{
    public required Guid Id { get; init; }

    public required Guid CategoryId { get; init; }

    public required Guid BrandId { get; init; }

    public required string Name { get; set; }

    public required string Slug { get; init; }

    public required string ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public required string Sku { get; init; }

    public decimal Price { get; set; }

    public decimal? WasPrice { get; set; }

    public decimal Rating { get; init; }

    public int ReviewCount { get; init; }

    public bool IsFeatured { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; set; }

    public Category Category { get; init; } = null!;

    public Brand Brand { get; init; } = null!;

    public ICollection<ProductImage> Images { get; init; } = [];

    public ICollection<ProductSpecification> Specifications { get; init; } = [];

    public ICollection<Inventory> Inventory { get; init; } = [];
}
