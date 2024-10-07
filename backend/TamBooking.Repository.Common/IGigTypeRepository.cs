using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface IGigTypeRepository
    {
        Task<List<GigType>> GetAsync();
    }
}