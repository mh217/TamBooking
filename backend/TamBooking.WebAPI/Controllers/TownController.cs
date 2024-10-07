using Microsoft.AspNetCore.Mvc;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [Route("api/towns")]
    [ApiController]
    public class TownController : ControllerBase
    {
        private readonly ITownService _townService;

        public TownController(ITownService townService)
        {
            _townService = townService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTowns()
        {
            List<TownName> townsGet = [];
            var towns = await _townService.GetTownsAsync();

            foreach (var town in towns)
            {
                TownName newTown = new()
                {
                    Id = town.Id,
                    Name = town.Name
                };
                townsGet.Add(newTown);
            }
            return Ok(townsGet);
        }
    }
}