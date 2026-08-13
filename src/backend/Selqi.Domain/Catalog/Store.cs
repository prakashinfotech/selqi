namespace Selqi.Domain.Catalog;

public sealed class Store
{
    public required Guid Id { get; init; }

    public required string Name { get; set; }

    public required string AddressLine1 { get; set; } = string.Empty;

    public string? AddressLine2 { get; set; }

    public required string Town { get; set; }

    public required string Postcode { get; set; }

    public string? Phone { get; set; }

    public StoreType StoreType { get; set; } = StoreType.Standalone;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; init; }

    public DateTime UpdatedAtUtc { get; set; }

    public ICollection<Inventory> Inventory { get; init; } = [];
}
