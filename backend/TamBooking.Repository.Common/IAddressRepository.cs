using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface IAddressRepository
    {
        Task<Guid> InputAddressAsync(Address address);
    }
}