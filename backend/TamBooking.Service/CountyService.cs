using TamBooking.Model;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class CountyService : ICountyService
    {
        private readonly ICountyRepository _countyRepository;

        public CountyService(ICountyRepository countyRepository)
        {
            _countyRepository = countyRepository;
        }

        public async Task<List<County>> GetAllCountiesAsync()
        {
            return await _countyRepository.GetAllCountiesAsync();
        }
    }
}