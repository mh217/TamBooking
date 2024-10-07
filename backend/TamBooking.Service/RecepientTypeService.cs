using Microsoft.AspNetCore.Http;
using TamBooking.Model;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class RecepientTypeService : IRecepientTypeService
    {
        private readonly IRecepientTypeRepository _recepientTypeRepository;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecepientTypeService(IRecepientTypeRepository recepientTypeRepository, IHttpContextAccessor httpContextAccessor, IJwtService jwtService)
        {
            _recepientTypeRepository = recepientTypeRepository;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<RecepientType>> GetRecepientTypesAsync()
        {
            var recepientTypes = await _recepientTypeRepository.GetRecepientTypesAsync();
            return recepientTypes;
        }
    }
}