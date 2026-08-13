using Selqi.Domain.Shopping;

namespace Selqi.Application.Auth;

public interface IAddressRepository
{
    Task<IReadOnlyList<Address>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Address?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Address address, CancellationToken ct = default);
    void Remove(Address address);
    Task SaveChangesAsync(CancellationToken ct = default);
}
