using Selqi.Application.Admin;
using Selqi.Application.Auth;
using Selqi.Application.Basket;
using Selqi.Application.Catalog;
using Selqi.Application.Email;
using Selqi.Application.Identity;
using Selqi.Application.Orders;
using Selqi.Application.Payments;
using Selqi.Application.Pdf;
using Selqi.Application.Recommendations;
using Selqi.Application.Wishlist;
using Selqi.Infrastructure.Ai;
using Selqi.Infrastructure.Admin;
using Selqi.Infrastructure.Catalog;
using Selqi.Infrastructure.Configuration;
using Selqi.Infrastructure.Email;
using Selqi.Infrastructure.Identity;
using Selqi.Infrastructure.Orders;
using Selqi.Infrastructure.Payments;
using Selqi.Infrastructure.Pdf;
using Selqi.Infrastructure.Persistence;
using Selqi.Infrastructure.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace Selqi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.Provider), "Database provider is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionStringName), "Connection string name is required.")
            .ValidateOnStart();

        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.Secret), "JWT secret is required.")
            .ValidateOnStart();

        services
            .AddOptions<AdminSeedOptions>()
            .Bind(configuration.GetSection(AdminSeedOptions.SectionName));

        services
            .AddOptions<EmailOptions>()
            .Bind(configuration.GetSection(EmailOptions.SectionName))
            .Validate(o => !string.IsNullOrWhiteSpace(o.Host), "Email:Host is required.")
            .ValidateOnStart();

        services
            .AddOptions<GeminiOptions>()
            .Bind(configuration.GetSection(GeminiOptions.SectionName));

        services
            .AddOptions<RecommendationOptions>()
            .Bind(configuration.GetSection(RecommendationOptions.SectionName));

        services
            .AddOptions<RazorpayOptions>()
            .Bind(configuration.GetSection(RazorpayOptions.SectionName));

        var databaseOptions = configuration
            .GetSection(DatabaseOptions.SectionName)
            .Get<DatabaseOptions>()
            ?? throw new InvalidOperationException("Database configuration is missing.");

        var connectionString = configuration.GetConnectionString(databaseOptions.ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{databaseOptions.ConnectionStringName}' is missing.");

        services.AddDbContext<SelqiDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddMemoryCache();
        services.AddHttpClient("Gemini");
        services.AddHttpClient("Razorpay");

        QuestPDF.Settings.License = LicenseType.Community;

        services.AddScoped<ICatalogReadRepository, EfCatalogReadRepository>();
        services.AddSingleton<IInvoicePdfService, QuestPdfInvoiceService>();
        services.AddScoped<IAdminRepository, EfAdminRepository>();
        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IPasswordResetRepository, EfPasswordResetRepository>();
        services.AddScoped<IAddressRepository, EfAddressRepository>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IBasketRepository, EfBasketRepository>();
        services.AddScoped<IWishlistRepository, EfWishlistRepository>();
        services.AddScoped<IOrderRepository, EfOrderRepository>();
        services.AddScoped<ISavedCardRepository, EfSavedCardRepository>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IAiRecommendationService, GeminiRecommendationService>();
        services.AddScoped<IRazorpayService, RazorpayService>();
        services.AddHostedService<AdminSeedService>();

        return services;
    }
}
