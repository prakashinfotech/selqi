using Selqi.Domain.Identity;

namespace Selqi.Application.Auth;

public interface IPasswordResetRepository
{
    Task AddAsync(PasswordResetToken token, CancellationToken ct = default);
    Task<PasswordResetToken?> FindByTokenAsync(string token, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
