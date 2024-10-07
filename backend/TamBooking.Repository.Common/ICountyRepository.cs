using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface ICountyRepository
    {
        Task<List<County>> GetAllCountiesAsync();
    }
}