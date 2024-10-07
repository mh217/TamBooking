using TamBooking.Model;

namespace TamBooking.Service.Common
{
    public interface IAddressService
    {
        Task<Guid> InputAddressAsync(Address address);
    }
}