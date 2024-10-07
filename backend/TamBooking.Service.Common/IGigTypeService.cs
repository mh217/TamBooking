using TamBooking.Model;

namespace TamBooking.Service.Common
{
    public interface IGigTypeService
    {
        Task<List<GigType>> GetAsync();
    }
}