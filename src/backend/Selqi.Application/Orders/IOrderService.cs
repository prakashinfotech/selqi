namespace Selqi.Application.Orders;

public interface IOrderService
{
    Task<OrderDetailDto> PlaceOrderAsync(Guid userId, CheckoutRequest request, CancellationToken ct = default);
    Task<OrderDetailDto> PlaceRazorpayOrderAsync(Guid userId, VerifyRazorpayPaymentRequest request, string razorpayPaymentId, CancellationToken ct = default);
    Task<IReadOnlyList<OrderSummaryDto>> GetOrdersAsync(Guid userId, CancellationToken ct = default);
    Task<OrderDetailDto> GetOrderAsync(Guid userId, string orderNumber, CancellationToken ct = default);
}
