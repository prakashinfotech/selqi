using Selqi.Domain.Identity;

namespace Selqi.Domain.Orders;

public class Order
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public decimal SubTotal { get; set; }
    public decimal DeliveryCharge { get; set; }
    public decimal Total { get; set; }

    // Delivery address snapshot
    public string DeliveryFirstName { get; set; } = string.Empty;
    public string DeliveryLastName { get; set; } = string.Empty;
    public string DeliveryLine1 { get; set; } = string.Empty;
    public string? DeliveryLine2 { get; set; }
    public string DeliveryTown { get; set; } = string.Empty;
    public string DeliveryCounty { get; set; } = string.Empty;
    public string DeliveryPostcode { get; set; } = string.Empty;
    public string DeliveryCountry { get; set; } = "United Kingdom";
    public string DeliveryPhone { get; set; } = string.Empty;

    // Payment
    public string PaymentMethod { get; set; } = "Card";   // "Card" | "Razorpay"
    public string PaymentReference { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = "Pending";

    public DateTime PlacedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = [];
    public ICollection<OrderStatusHistory> StatusHistory { get; set; } = [];
}
