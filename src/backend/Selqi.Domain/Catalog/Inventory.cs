namespace Selqi.Domain.Catalog;

public sealed class Inventory
{
    public required Guid Id { get; init; }

    public required Guid ProductId { get; init; }

    public required Guid StoreId { get; init; }

    public int QuantityAvailable { get; set; }

    public int LowStockThreshold { get; set; } = 5;

    public DateTime? LastRestockedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public Product Product { get; init; } = null!;

    public Store Store { get; init; } = null!;
}
