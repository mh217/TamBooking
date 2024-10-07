using Microsoft.AspNetCore.Mvc;
using TamBooking.Model;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [ApiController]
    [Route("api/counties")]
    public class CountyController : ControllerBase
    {
        private readonly ICountyService _countyService;

        public CountyController(ICountyService countyService)
        {
            _countyService = countyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountiesAsync()
        {
            List<County> counties = await _countyService.GetAllCountiesAsync();
            return Ok(
                counties.Select(county => new CountyGet
                {
                    Id = county.Id,
                    Name = county.Name
                }).ToList());
        }
    }
}