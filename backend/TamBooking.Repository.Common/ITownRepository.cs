using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface ITownRepository
    {
        public Task<List<Town>> GetTownsAsync();
    }
}