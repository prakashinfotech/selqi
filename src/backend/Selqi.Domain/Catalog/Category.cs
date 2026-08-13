namespace Selqi.Domain.Catalog;

public sealed class Category
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Slug { get; init; }

    public string? Description { get; init; }

    public string? ImageUrl { get; init; }

    public Guid? ParentCategoryId { get; init; }

    public int DisplayOrder { get; init; }

    public bool IsDeleted { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }

    public Category? ParentCategory { get; init; }

    public ICollection<Category> SubCategories { get; init; } = [];

    public ICollection<Product> Products { get; init; } = [];
}
