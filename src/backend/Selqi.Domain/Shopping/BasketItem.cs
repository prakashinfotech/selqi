using Selqi.Domain.Catalog;

namespace Selqi.Domain.Shopping;

public class BasketItem
{
    public Guid Id { get; set; }
    public Guid BasketId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime AddedAtUtc { get; set; }

    public Basket Basket { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
