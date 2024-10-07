using TamBooking.Model;

namespace TamBooking.Service.Common
{
    public interface ICountyService
    {
        Task<List<County>> GetAllCountiesAsync();
    }
}