using Selqi.Domain.Identity;

namespace Selqi.Application.Auth;

public interface IUserRepository
{
    Task<ApplicationUser?> FindByEmailAsync(string email, CancellationToken ct = default);
    Task<ApplicationUser?> FindByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
    Task AddAsync(ApplicationUser user, CancellationToken ct = default);
    Task UpdateAsync(ApplicationUser user, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
