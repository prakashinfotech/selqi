using Selqi.Domain.Catalog;
using Selqi.Domain.Identity;

namespace Selqi.Domain.Shopping;

public class WishlistItem
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public DateTime AddedAtUtc { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
