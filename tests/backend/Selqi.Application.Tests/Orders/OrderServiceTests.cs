using Selqi.Application.Basket;
using Selqi.Application.Catalog;
using Selqi.Application.Identity;
using Selqi.Application.Orders;
using Selqi.Domain.Catalog;
using Selqi.Domain.Identity;
using Selqi.Domain.Orders;
using Selqi.Domain.Shopping;
using Xunit;

namespace Selqi.Application.Tests.Orders;

public sealed class OrderServiceTests
{
    private static readonly DeliveryAddressDto ValidAddress = new(
        "Alice", "Smith", "123 High Street", null,
        "London", null, "SW1A 1AA", "+447911123456");

    // ── Positive: PlaceOrder ────────────────────────────────────────────────

    [Fact]
    public async Task PlaceOrderAsync_WithNewCard_CreatesOrderWithMockPaymentRef()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var (service, orderRepo, _) = BuildService(userId, productId, unitPrice: 20m, qty: 2);

        var request = new CheckoutRequest(
            ValidAddress,
            "standard",
            new PaymentDto("4111111111111111", "Alice Smith", "12", "27",
                null, false, "Visa"));

        await service.PlaceOrderAsync(userId, request);

        var saved = Assert.Single(orderRepo.Orders);
        Assert.StartsWith("MOCK-", saved.PaymentReference);
        Assert.Equal(40m, saved.SubTotal);
        Assert.Equal(OrderStatus.Pending, saved.Status);
    }

    [Fact]
    public async Task PlaceOrderAsync_StandardDelivery_BelowFreeThreshold_AddsStandardCharge()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var (service, orderRepo, _) = BuildService(userId, productId, unitPrice: 10m, qty: 1);

        await service.PlaceOrderAsync(userId, new CheckoutRequest(
            ValidAddress, "standard",
            new PaymentDto("1234", "Alice Smith", "12", "27", null, false, "Visa")));

        var saved = Assert.Single(orderRepo.Orders);
        Assert.Equal(3.95m, saved.DeliveryCharge);
    }

    [Fact]
    public async Task PlaceOrderAsync_StandardDelivery_AboveFreeThreshold_HasZeroDeliveryCharge()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var (service, orderRepo, _) = BuildService(userId, productId, unitPrice: 60m, qty: 1);

        await service.PlaceOrderAsync(userId, new CheckoutRequest(
            ValidAddress, "standard",
            new PaymentDto("1234", "Alice Smith", "12", "27", null, false, "Visa")));

        var saved = Assert.Single(orderRepo.Orders);
        Assert.Equal(0m, saved.DeliveryCharge);
    }

    [Fact]
    public async Task PlaceOrderAsync_ExpressDelivery_AddsExpressCharge()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var (service, orderRepo, _) = BuildService(userId, productId, unitPrice: 20m, qty: 1);

        await service.PlaceOrderAsync(userId, new CheckoutRequest(
            ValidAddress, "express",
            new PaymentDto("1234", "Alice Smith", "12", "27", null, false, "Visa")));

        var saved = Assert.Single(orderRepo.Orders);
        Assert.Equal(5.95m, saved.DeliveryCharge);
    }

    [Fact]
    public async Task PlaceOrderAsync_AddsInitialStatusHistoryEntry()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var (service, orderRepo, _) = BuildService(userId, productId, unitPrice: 20m, qty: 1);

        await service.PlaceOrderAsync(userId, new CheckoutRequest(
            ValidAddress, "standard",
            new PaymentDto("1234", "Alice", "12", "27", null, false, "Visa")));

        var saved = Assert.Single(orderRepo.Orders);
        Assert.Single(saved.StatusHistory);
    }

    // ── Negative: PlaceOrder ────────────────────────────────────────────────

    [Fact]
    public async Task PlaceOrderAsync_WithEmptyBasket_ThrowsInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        var basketRepo = new FakeBasketRepository(null);
        var service = new OrderService(
            new FakeOrderRepository(),
            new FakeBasketService(),
            basketRepo,
            new FakeSavedCardRepository());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.PlaceOrderAsync(userId, new CheckoutRequest(
                ValidAddress, "standard",
                new PaymentDto("1234", "Alice", "12", "27", null, false, "Visa"))));
    }

    [Fact]
    public async Task PlaceOrderAsync_WithNoCardDetails_ThrowsInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var (service, _, _) = BuildService(userId, productId, unitPrice: 20m, qty: 1);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.PlaceOrderAsync(userId, new CheckoutRequest(
                ValidAddress, "standard",
                new PaymentDto(null, null, null, null, null, false, null))));
    }

    // ── Positive: PlaceRazorpayOrder ────────────────────────────────────────

    [Fact]
    public async Task PlaceRazorpayOrderAsync_WithValidBasket_CreatesConfirmedOrder()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var (service, orderRepo, _) = BuildService(userId, productId, unitPrice: 30m, qty: 2);

        var request = new VerifyRazorpayPaymentRequest(
            "rzp_order_123", "pay_abc", "sig_xyz",
            ValidAddress, "standard");

        await service.PlaceRazorpayOrderAsync(userId, request, "pay_abc");

        var saved = Assert.Single(orderRepo.Orders);
        Assert.Equal(OrderStatus.Confirmed, saved.Status);
        Assert.Equal("Razorpay", saved.PaymentMethod);
        Assert.Equal("Captured", saved.PaymentStatus);
        Assert.StartsWith("rzp_", saved.PaymentReference);
    }

    [Fact]
    public async Task PlaceRazorpayOrderAsync_PaymentRefContainsOrderAndPaymentId()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var (service, orderRepo, _) = BuildService(userId, productId, unitPrice: 30m, qty: 1);

        var request = new VerifyRazorpayPaymentRequest(
            "rzp_order_ABC", "pay_XYZ", "sig",
            ValidAddress, "standard");

        await service.PlaceRazorpayOrderAsync(userId, request, "pay_XYZ");

        var saved = Assert.Single(orderRepo.Orders);
        Assert.Contains("rzp_order_ABC", saved.PaymentReference);
        Assert.Contains("pay_XYZ", saved.PaymentReference);
    }

    // ── Negative: PlaceRazorpayOrder ────────────────────────────────────────

    [Fact]
    public async Task PlaceRazorpayOrderAsync_WithEmptyBasket_ThrowsInvalidOperationException()
    {
        var basketRepo = new FakeBasketRepository(null);
        var service = new OrderService(
            new FakeOrderRepository(),
            new FakeBasketService(),
            basketRepo,
            new FakeSavedCardRepository());

        var request = new VerifyRazorpayPaymentRequest(
            "rzp_order_123", "pay_abc", "sig",
            ValidAddress, "standard");

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.PlaceRazorpayOrderAsync(Guid.NewGuid(), request, "pay_abc"));
    }

    // ── Positive: GetOrder ──────────────────────────────────────────────────

    [Fact]
    public async Task GetOrderAsync_WithValidOwner_ReturnsOrderDetail()
    {
        var userId = Guid.NewGuid();
        var order = BuildOrder(userId, "ORD-2026-99999");
        var orderRepo = new FakeOrderRepository([order]);
        var service = new OrderService(
            orderRepo, new FakeBasketService(),
            new FakeBasketRepository(null), new FakeSavedCardRepository());

        var detail = await service.GetOrderAsync(userId, "ORD-2026-99999");

        Assert.Equal("ORD-2026-99999", detail.OrderNumber);
    }

    // ── Negative: GetOrder ──────────────────────────────────────────────────

    [Fact]
    public async Task GetOrderAsync_ForWrongUser_ThrowsUnauthorizedAccessException()
    {
        var ownerId = Guid.NewGuid();
        var order = BuildOrder(ownerId, "ORD-2026-11111");
        var orderRepo = new FakeOrderRepository([order]);
        var service = new OrderService(
            orderRepo, new FakeBasketService(),
            new FakeBasketRepository(null), new FakeSavedCardRepository());

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.GetOrderAsync(Guid.NewGuid(), "ORD-2026-11111"));
    }

    [Fact]
    public async Task GetOrderAsync_WithNonExistentOrderNumber_ThrowsKeyNotFoundException()
    {
        var service = new OrderService(
            new FakeOrderRepository(),
            new FakeBasketService(),
            new FakeBasketRepository(null),
            new FakeSavedCardRepository());

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.GetOrderAsync(Guid.NewGuid(), "ORD-0000-00000"));
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private static (OrderService, FakeOrderRepository, FakeBasketRepository) BuildService(
        Guid userId, Guid productId, decimal unitPrice, int qty)
    {
        var basket = new Domain.Shopping.Basket
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Items =
            [
                new BasketItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    Quantity = qty,
                    UnitPrice = unitPrice,
                    Product = new Product
                    {
                        Id = productId,
                        CategoryId = Guid.NewGuid(),
                        BrandId = Guid.NewGuid(),
                        Name = "Test Product",
                        Slug = "test-product",
                        ShortDescription = "Desc",
                        Sku = "SKU-T1",
                        Price = unitPrice
                    }
                }
            ]
        };

        var basketRepo = new FakeBasketRepository(basket);
        var orderRepo = new FakeOrderRepository();
        var service = new OrderService(
            orderRepo, new FakeBasketService(), basketRepo, new FakeSavedCardRepository());

        return (service, orderRepo, basketRepo);
    }

    private static Order BuildOrder(Guid userId, string orderNumber) => new()
    {
        Id = Guid.NewGuid(),
        OrderNumber = orderNumber,
        UserId = userId,
        Status = OrderStatus.Pending,
        SubTotal = 20m,
        DeliveryCharge = 3.95m,
        Total = 23.95m,
        DeliveryFirstName = "Alice",
        DeliveryLastName = "Smith",
        DeliveryLine1 = "123 High Street",
        DeliveryTown = "London",
        DeliveryCounty = string.Empty,
        DeliveryPostcode = "SW1A 1AA",
        DeliveryPhone = "+447911123456",
        PaymentReference = "mock_card_test",
        PaymentStatus = "Authorised",
        PlacedAtUtc = DateTime.UtcNow,
        UpdatedAtUtc = DateTime.UtcNow
    };

    // ── Fakes ────────────────────────────────────────────────────────────────

    private sealed class FakeOrderRepository(IReadOnlyList<Order>? seeded = null) : IOrderRepository
    {
        public List<Order> Orders { get; } = seeded?.ToList() ?? [];

        public Task<IReadOnlyList<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
            => Task.FromResult<IReadOnlyList<Order>>(Orders.Where(o => o.UserId == userId).ToList());

        public Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default)
            => Task.FromResult(Orders.FirstOrDefault(o => o.OrderNumber == orderNumber));

        public Task AddAsync(Order order, CancellationToken ct = default)
        {
            Orders.Add(order);
            return Task.CompletedTask;
        }

        public void AddStatusHistory(OrderStatusHistory entry) { }

        public Task SaveChangesAsync(CancellationToken ct = default)
            => Task.CompletedTask;
    }

    private sealed class FakeBasketRepository(Domain.Shopping.Basket? basket) : IBasketRepository
    {
        public Task<Domain.Shopping.Basket?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
            => Task.FromResult(basket);

        public Task<Domain.Shopping.Basket> GetOrCreateAsync(Guid userId, CancellationToken ct = default)
        {
            basket ??= new Domain.Shopping.Basket { Id = Guid.NewGuid(), UserId = userId };
            return Task.FromResult(basket);
        }

        public Task<BasketItem?> GetItemAsync(Guid itemId, CancellationToken ct = default)
            => Task.FromResult(basket?.Items.FirstOrDefault(i => i.Id == itemId));

        public Task AddItemAsync(BasketItem item, CancellationToken ct = default)
        {
            basket?.Items.Add(item);
            return Task.CompletedTask;
        }

        public Task RemoveItemAsync(BasketItem item, CancellationToken ct = default)
        {
            basket?.Items.Remove(item);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
            => Task.CompletedTask;
    }

    private sealed class FakeBasketService : IBasketService
    {
        public Task ClearAsync(Guid userId, CancellationToken ct = default)
            => Task.CompletedTask;

        public Task<BasketDto> GetAsync(Guid userId, CancellationToken ct = default)
            => Task.FromResult(new BasketDto(Guid.Empty, [], 0, 0m));

        public Task<BasketDto> AddItemAsync(Guid userId, AddToBasketRequest request, CancellationToken ct = default)
            => Task.FromResult(new BasketDto(Guid.Empty, [], 0, 0m));

        public Task<BasketDto> UpdateItemAsync(Guid userId, Guid itemId, UpdateBasketItemRequest request, CancellationToken ct = default)
            => Task.FromResult(new BasketDto(Guid.Empty, [], 0, 0m));

        public Task<BasketDto> RemoveItemAsync(Guid userId, Guid itemId, CancellationToken ct = default)
            => Task.FromResult(new BasketDto(Guid.Empty, [], 0, 0m));
    }

    private sealed class FakeSavedCardRepository : ISavedCardRepository
    {
        private readonly List<SavedCard> _store = [];

        public Task<IReadOnlyList<SavedCard>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
            => Task.FromResult<IReadOnlyList<SavedCard>>(_store.Where(c => c.UserId == userId).ToList());

        public Task<SavedCard?> GetByIdAsync(Guid cardId, CancellationToken ct = default)
            => Task.FromResult(_store.FirstOrDefault(c => c.Id == cardId));

        public void Add(SavedCard card) => _store.Add(card);
        public void Remove(SavedCard card) => _store.Remove(card);

        public Task SaveChangesAsync(CancellationToken ct = default)
            => Task.CompletedTask;
    }
}
