using Selqi.Application.Auth;
using Selqi.Domain.Identity;
using Selqi.Infrastructure.Configuration;
using Selqi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;



namespace Selqi.Infrastructure.Identity;

internal sealed class AdminSeedService(
    IServiceScopeFactory scopeFactory,
    IOptions<AdminSeedOptions> options,
    ILogger<AdminSeedService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var adminSeed = options.Value;
        if (!adminSeed.Enabled)
            return;

        if (string.IsNullOrWhiteSpace(adminSeed.Email) || string.IsNullOrWhiteSpace(adminSeed.Password))
        {
            logger.LogWarning("Admin seed is enabled but email or password is missing.");
            return;
        }

        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SelqiDbContext>();
        var email = adminSeed.Email.Trim().ToLowerInvariant();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (user is null)
        {
            db.Users.Add(new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = PasswordHasher.Hash(adminSeed.Password),
                FirstName = adminSeed.FirstName.Trim(),
                LastName = adminSeed.LastName.Trim(),
                Role = UserRoles.Admin,
                IsEmailVerified = true,
                CreatedAtUtc = DateTime.UtcNow
            });
        }
        else if (user.Role != UserRoles.Admin)
        {
            user.Role = UserRoles.Admin;
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
