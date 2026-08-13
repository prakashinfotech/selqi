using Selqi.Application.Auth;
using Selqi.Application.Admin;
using Selqi.Application.Basket;
using Selqi.Application.Catalog;
using Selqi.Application.Identity;
using Selqi.Application.Orders;
using Selqi.Application.Wishlist;
using Microsoft.Extensions.DependencyInjection;

namespace Selqi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IWishlistService, WishlistService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ISavedCardService, SavedCardService>();

        return services;
    }
}
