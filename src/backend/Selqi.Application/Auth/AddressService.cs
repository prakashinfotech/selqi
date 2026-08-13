using Selqi.Domain.Shopping;

namespace Selqi.Application.Auth;

public sealed class AddressService(IAddressRepository addresses) : IAddressService
{
    public async Task<IReadOnlyList<AddressDto>> GetAddressesAsync(Guid userId, CancellationToken ct = default)
    {
        var list = await addresses.GetByUserIdAsync(userId, ct);
        return list.Select(ToDto).ToList();
    }

    public async Task<AddressDto> SaveAddressAsync(Guid userId, SaveAddressRequest request, CancellationToken ct = default)
    {
        if (request.IsDefault)
            await ClearDefaultAsync(userId, ct);

        var address = new Address
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Label = request.Label.Trim(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Line1 = request.Line1.Trim(),
            Line2 = request.Line2?.Trim(),
            Town = request.Town.Trim(),
            County = request.County?.Trim() ?? string.Empty,
            Postcode = request.Postcode.Trim().ToUpperInvariant(),
            Country = (request.Country ?? "United Kingdom").Trim(),
            Phone = request.Phone?.Trim() ?? string.Empty,
            IsDefault = request.IsDefault,
            CreatedAtUtc = DateTime.UtcNow
        };

        await addresses.AddAsync(address, ct);
        await addresses.SaveChangesAsync(ct);
        return ToDto(address);
    }

    public async Task<AddressDto> UpdateAddressAsync(Guid userId, Guid addressId, UpdateAddressRequest request, CancellationToken ct = default)
    {
        var address = await addresses.GetByIdAsync(addressId, ct)
            ?? throw new KeyNotFoundException("Address not found.");

        if (address.UserId != userId)
            throw new UnauthorizedAccessException("Access denied.");

        if (request.IsDefault)
            await ClearDefaultAsync(userId, ct);

        address.Label = request.Label.Trim();
        address.FirstName = request.FirstName.Trim();
        address.LastName = request.LastName.Trim();
        address.Line1 = request.Line1.Trim();
        address.Line2 = request.Line2?.Trim();
        address.Town = request.Town.Trim();
        address.County = request.County?.Trim() ?? string.Empty;
        address.Postcode = request.Postcode.Trim().ToUpperInvariant();
        address.Country = (request.Country ?? "United Kingdom").Trim();
        address.Phone = request.Phone?.Trim() ?? string.Empty;
        address.IsDefault = request.IsDefault;

        await addresses.SaveChangesAsync(ct);
        return ToDto(address);
    }

    public async Task DeleteAddressAsync(Guid userId, Guid addressId, CancellationToken ct = default)
    {
        var address = await addresses.GetByIdAsync(addressId, ct)
            ?? throw new KeyNotFoundException("Address not found.");

        if (address.UserId != userId)
            throw new UnauthorizedAccessException("Access denied.");

        addresses.Remove(address);
        await addresses.SaveChangesAsync(ct);
    }

    public async Task SetDefaultAsync(Guid userId, Guid addressId, CancellationToken ct = default)
    {
        var address = await addresses.GetByIdAsync(addressId, ct)
            ?? throw new KeyNotFoundException("Address not found.");

        if (address.UserId != userId)
            throw new UnauthorizedAccessException("Access denied.");

        await ClearDefaultAsync(userId, ct);
        address.IsDefault = true;
        await addresses.SaveChangesAsync(ct);
    }

    private async Task ClearDefaultAsync(Guid userId, CancellationToken ct)
    {
        var all = await addresses.GetByUserIdAsync(userId, ct);
        foreach (var a in all.Where(a => a.IsDefault))
            a.IsDefault = false;
    }

    private static AddressDto ToDto(Address a) =>
        new(a.Id, a.Label, a.FirstName, a.LastName, a.Line1, a.Line2,
            a.Town, a.County, a.Postcode, a.Country, a.Phone, a.IsDefault);
}
