using TamBooking.Model;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IJwtService _jwtService;

        public AddressService(IAddressRepository addressRepository, IJwtService jwtService)
        {
            _addressRepository = addressRepository;
            _jwtService = jwtService;
        }

        public async Task<Guid> InputAddressAsync(Address address)
        {
            var currentUserId = Guid.Parse(_jwtService.GetCurrentUserClaims().Id);
            address.CreatedByUserId = currentUserId;
            address.UpdatedByUserId = currentUserId;
            address.IsActive = true;
            address.TownId = address.TownId;
            var addressId = await _addressRepository.InputAddressAsync(address);

            return addressId;
        }
    }
}