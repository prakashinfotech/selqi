using Selqi.Domain.Orders;
using Selqi.Domain.Shopping;

namespace Selqi.Domain.Identity;

public class ApplicationUser
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = UserRoles.Customer;
    public bool IsEmailVerified { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? LastLoginAtUtc { get; set; }

    public ICollection<Address> Addresses { get; set; } = [];
    public ICollection<SavedCard> SavedCards { get; set; } = [];
    public ICollection<WishlistItem> WishlistItems { get; set; } = [];
    public Basket? Basket { get; set; }
    public ICollection<Order> Orders { get; set; } = [];
}
