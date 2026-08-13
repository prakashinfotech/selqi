namespace Selqi.Application.Constants;

public static class DashboardConstants
{
    public const int RecentOrdersCount = 5;

    public static readonly Domain.Orders.OrderStatus[] OpenOrderStatuses =
    [
        Domain.Orders.OrderStatus.Pending,
        Domain.Orders.OrderStatus.Confirmed,
        Domain.Orders.OrderStatus.Processing
    ];
}
