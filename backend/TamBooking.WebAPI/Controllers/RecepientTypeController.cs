using Microsoft.AspNetCore.Mvc;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [Route("api/recepientTypes")]
    [ApiController]
    public class RecepientTypeController : ControllerBase
    {
        private readonly IRecepientTypeService _recepientTypeService;

        public RecepientTypeController(IRecepientTypeService recepientTypeService)
        {
            _recepientTypeService = recepientTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecepientTypesAsync()
        {
            var recepientType = await _recepientTypeService.GetRecepientTypesAsync();
            List<GetRecepientType> types = [];

            foreach (var type in recepientType)
            {
                GetRecepientType newType = new()
                {
                    Id = type.Id,
                    Name = type.Name
                };
                types.Add(newType);
            }
            return Ok(types);
        }
    }
}