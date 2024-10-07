using TamBooking.Model;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class GigTypeService : IGigTypeService
    {
        private readonly IGigTypeRepository _gigTypeRepository;

        public GigTypeService(IGigTypeRepository gigTypeRepository)
        {
            _gigTypeRepository = gigTypeRepository;
        }

        public async Task<List<GigType>> GetAsync()
        {
            var gigTypes = new List<GigType>();
            var currentGigTypes = await _gigTypeRepository.GetAsync();
            foreach (var currentGigType in currentGigTypes)
            {
                var gigType = new GigType();
                gigType.Id = currentGigType.Id;
                gigType.Name = currentGigType.Name;
                gigTypes.Add(gigType);
            }
            return gigTypes;
        }
    }
}