using Selqi.Application.Auth;
using Selqi.Domain.Shopping;
using Selqi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Selqi.Infrastructure.Identity;

internal sealed class EfAddressRepository(SelqiDbContext db) : IAddressRepository
{
    public async Task<IReadOnlyList<Address>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await db.Addresses
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
            .ThenBy(a => a.Label)
            .ToListAsync(ct);
    }

    public Task<Address?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => db.Addresses.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task AddAsync(Address address, CancellationToken ct = default)
        => await db.Addresses.AddAsync(address, ct);

    public void Remove(Address address)
        => db.Addresses.Remove(address);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}
