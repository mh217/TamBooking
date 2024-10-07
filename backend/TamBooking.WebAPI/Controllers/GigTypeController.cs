using Microsoft.AspNetCore.Mvc;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [Route("api/gigTypes")]
    [ApiController]
    public class GigTypeController : ControllerBase
    {
        private readonly IGigTypeService _gigTypeService;

        public GigTypeController(IGigTypeService gigTypeService)
        {
            _gigTypeService = gigTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var gigTypes = new List<RESTGigTypeGet>();
            var currentGigTypes = await _gigTypeService.GetAsync();
            foreach (var type in currentGigTypes)
            {
                var gigType = new RESTGigTypeGet
                {
                    Id = type.Id,
                    Name = type.Name
                };
                gigTypes.Add(gigType);
            }
            return Ok(gigTypes);
        }
    }
}