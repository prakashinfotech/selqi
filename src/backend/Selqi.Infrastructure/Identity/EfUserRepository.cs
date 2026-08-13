using Selqi.Application.Auth;
using Selqi.Domain.Identity;
using Selqi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Selqi.Infrastructure.Identity;

internal sealed class EfUserRepository(SelqiDbContext db) : IUserRepository
{
    public Task<ApplicationUser?> FindByEmailAsync(string email, CancellationToken ct = default)
        => db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<ApplicationUser?> FindByIdAsync(Guid id, CancellationToken ct = default)
        => db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        => db.Users.AnyAsync(u => u.Email == email, ct);

    public Task AddAsync(ApplicationUser user, CancellationToken ct = default)
    {
        db.Users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ApplicationUser user, CancellationToken ct = default)
    {
        db.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}
