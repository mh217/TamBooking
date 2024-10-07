using TamBooking.Model;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class TownService : ITownService
    {
        private readonly ITownRepository _townRepository;

        public TownService(ITownRepository repository)
        {
            _townRepository = repository;
        }

        public async Task<List<Town>> GetTownsAsync()
        {
            var towns = await _townRepository.GetTownsAsync();
            return towns;
        }
    }
}