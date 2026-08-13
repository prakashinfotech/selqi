namespace Selqi.Application.Auth;

public interface IAddressService
{
    Task<IReadOnlyList<AddressDto>> GetAddressesAsync(Guid userId, CancellationToken ct = default);
    Task<AddressDto> SaveAddressAsync(Guid userId, SaveAddressRequest request, CancellationToken ct = default);
    Task<AddressDto> UpdateAddressAsync(Guid userId, Guid addressId, UpdateAddressRequest request, CancellationToken ct = default);
    Task DeleteAddressAsync(Guid userId, Guid addressId, CancellationToken ct = default);
    Task SetDefaultAsync(Guid userId, Guid addressId, CancellationToken ct = default);
}
