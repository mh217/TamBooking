using Microsoft.AspNetCore.Mvc;
using TamBooking.Model;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost]
        [Route("insertAddress")]
        public async Task<IActionResult> InputAddressAsync([FromBody] RESTAddressCreate address)
        {
            Address inputAddress = new()
            {
                Line = address.Line,
                Suite = address.Suite,
                BuildingNumber = address.BuildingNumber,
                TownId = address.TownId
            };

            var addressId = await _addressService.InputAddressAsync(inputAddress);
            return Ok(new { addressId });
        }
    }
}