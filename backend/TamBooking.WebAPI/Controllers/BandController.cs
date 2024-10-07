using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using TamBooking.Common;
using TamBooking.Model;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [Route("api/bands")]
    [ApiController]
    public class BandController : ControllerBase
    {
        private readonly IBandService _bandService;

        public BandController(IBandService bandService)
        {
            _bandService = bandService;
        }

        [HttpGet]
        [Route("GetAllBands")]
        public async Task<IActionResult> GetAllBandsCountAsync([FromQuery] string searchQuery = "", [FromQuery] decimal priceFrom = 0, [FromQuery] decimal priceTo = 0, [FromQuery] Guid? countyId = null)
        {
            BandFilter filter = new BandFilter();
            filter.SearchQuery = searchQuery;
            filter.PriceFrom = priceFrom;
            filter.PriceTo = priceTo;
            filter.CountyId = countyId;

            var currentBands = await _bandService.GetAllBandsCountAsync(filter);
            return Ok(currentBands.Count);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllBandsAsync([FromQuery] string searchQuery = "", [FromQuery] Guid? id = null,
            [FromQuery] decimal priceFrom = 0, [FromQuery] decimal priceTo = 0, [FromQuery] Guid? countyId = null,
            [FromQuery] string orderBy = "BandName", [FromQuery] string sortDirection = "DESC", [FromQuery] int rpp = 5, [FromQuery] int pageNumber = 1)
        {
            BandFilter filter = new BandFilter();
            filter.SearchQuery = searchQuery;
            filter.PriceFrom = priceFrom;
            filter.PriceTo = priceTo;
            filter.CountyId = countyId;
            filter.Id = id;

            Sorting sorting = new Sorting();
            sorting.OrderDirection = sortDirection;
            sorting.OrderBy = orderBy;

            Paging paging = new Paging();
            paging.PageNumber = pageNumber;
            paging.Rpp = rpp;

            var currentBands = await _bandService.GetAllBandsAsync(filter, paging, sorting);
            List<BandGet> bands = new List<BandGet>();
            foreach (var band in currentBands)
            {
                var getBand = new BandGet();
                getBand.Id = band.Id;
                getBand.Price = band.Price;
                getBand.Name = band.Name;
                getBand.TownId = band.Town.Id;

                bands.Add(getBand);
            }
            if (bands != null)
            {
                return Ok(bands);
            }
            return Ok("There are no matching bands!");
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "band")]
        public async Task<IActionResult> DeleteBandAsync()
        {
            bool isDeleted = await _bandService.DeleteBandAsync();
            if (isDeleted)
            {
                return Ok("Band deleted successfully!");
            }
            return NotFound("There is no such band!");
        }

        [HttpPut]
        [Route("update/{id}")]
        [Authorize(Roles = "band")]
        public async Task<IActionResult> UpdateBandAsync([FromBody] BandUpdate bandUpdate)
        {
            Band band = new()
            {
                Name = bandUpdate.Name,
                Price = bandUpdate.Price,
                TownId = bandUpdate.TownId
            };

            if (await _bandService.UpdateBandAsync(band))
            {
                return Ok();
            }
            return NotFound("Band not found!");
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateBandAsync(BandInfo bandInfo)
        {
            Band band = new Band();
            band.Name = bandInfo.Name;
            band.Price = bandInfo.Price;
            band.TownId = bandInfo.TownId;
            await _bandService.CreateBandAsync(band);
            return StatusCode(201);
        }
    }
}