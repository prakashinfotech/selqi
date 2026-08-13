using Selqi.Domain.Identity;

namespace Selqi.Domain.Shopping;

public class Basket
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public ICollection<BasketItem> Items { get; set; } = [];
}
