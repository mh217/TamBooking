using TamBooking.Model;

namespace TamBooking.Service.Common
{
    public interface ITownService
    {
        public Task<List<Town>> GetTownsAsync();
    }
}