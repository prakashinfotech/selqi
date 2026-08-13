using Selqi.Application.Auth;
using Selqi.Domain.Identity;
using Selqi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Selqi.Infrastructure.Identity;

public sealed class EfPasswordResetRepository(SelqiDbContext db) : IPasswordResetRepository
{
    public async Task AddAsync(PasswordResetToken token, CancellationToken ct = default) =>
        await db.PasswordResetTokens.AddAsync(token, ct);

    public Task<PasswordResetToken?> FindByTokenAsync(string token, CancellationToken ct = default) =>
        db.PasswordResetTokens
            .FirstOrDefaultAsync(t => t.Token == token, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
