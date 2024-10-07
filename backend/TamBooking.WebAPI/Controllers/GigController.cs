using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TamBooking.Model;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [Route("api/gigs")]
    [ApiController]
    public class GigController : ControllerBase
    {
        private readonly IGigService _gigService;

        public GigController(IGigService gigService)
        {
            _gigService = gigService;
        }

        [HttpPost]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> CreateGigAsync(CreateGig newGig)
        {
            Gig gig = new()
            {
                OccasionDate = newGig.OccasionDate,
                AddressId = newGig.AddressId,
                TypeId = newGig.TypeId,
                BandId = newGig.BandId
            };
            var gigId = await _gigService.CreateGigAsync(gig);
            return Ok(new { gigId });
        }

        [HttpGet]
        [Route("getBandShortInfo/{id}")]
        public async Task<IActionResult> GetBandShortInfosAsync(Guid id)
        {
            List<GigBandShortInfo> gigsGet = [];
            var gigs = await _gigService.GetGigsAsync(id);
            foreach (var foundGig in gigs)
            {
                Client client = new();
                GigBandShortInfo gig = new()
                {
                    OccasionDate = foundGig.OccasionDate,
                    ClientId = foundGig.ClientId
                };
                client.FirstName = foundGig.Client.FirstName;
                client.LastName = foundGig.Client.LastName;
                gig.Client = client;
                gig.Id = foundGig.Id;
                gigsGet.Add(gig);
            }
            var lastThreeGigs = gigsGet.Take(3).ToList();
            return Ok(lastThreeGigs);
        }

        [HttpGet]
        [Route("getClientShortInfo/{id}")]
        public async Task<IActionResult> GetClientShortInfosAsync(Guid id)
        {
            List<GigClientShortInfo> gigsGet = new List<GigClientShortInfo>();
            var gigs = await _gigService.GetGigsAsync(id);
            foreach (var foundGig in gigs)
            {
                Band band = new Band();
                GigClientShortInfo gig = new GigClientShortInfo();
                gig.OccasionDate = foundGig.OccasionDate;
                gig.BandId = foundGig.BandId;
                band.Name = foundGig.Band.Name;
                gig.Band = band;
                gig.Id = foundGig.Id;
                gigsGet.Add(gig);
            }
            var lastThreeGigs = gigsGet.Take(3).ToList();
            return Ok(lastThreeGigs);
        }

        [HttpGet]
        [Route("getBandMoreInfo/{id}")]
        public async Task<IActionResult> GetBandMoreInfosAsync(Guid id)
        {
            List<GigBandMoreInfo> gigsGet = [];
            var gigs = await _gigService.GetGigsAsync(id);
            if (gigs is null)
            {
                return BadRequest("No gigs found");
            }
            foreach (var foundGig in gigs)
            {
                Client client = new();
                Address address = new();
                GigType type = new();
                GigBandMoreInfo gig = new()
                {
                    OccasionDate = foundGig.OccasionDate
                };
                client.FirstName = foundGig.Client.FirstName;
                client.LastName = foundGig.Client.LastName;
                address.Line = foundGig.Address.Line;
                address.BuildingNumber = foundGig.Address.BuildingNumber;
                address.Suite = foundGig.Address.Suite;
                type.Name = foundGig.GigType.Name;

                gig.Id = foundGig.Id;
                gig.Client = client;
                gig.Address = address;
                gig.GigType = type;
                gigsGet.Add(gig);
            }
            return Ok(gigsGet);
        }

        [HttpGet]
        [Route("getClientMoreInfo/{id}")]
        public async Task<IActionResult> GetClientMoreInfosAsync(Guid id)
        {
            List<GigClientMoreInfo> gigsGet = new List<GigClientMoreInfo>();
            var gigs = await _gigService.GetGigsAsync(id);
            foreach (var foundGig in gigs)
            {
                Band band = new Band();
                Address address = new Address();
                GigType type = new GigType();
                GigClientMoreInfo gig = new GigClientMoreInfo();

                gig.OccasionDate = foundGig.OccasionDate;
                gig.Id = foundGig.Id;
                band.Id = foundGig.Band.Id;
                band.Name = foundGig.Band.Name;
                band.Price = foundGig.Band.Price;
                address.Line = foundGig.Address.Line;
                address.BuildingNumber = foundGig.Address.BuildingNumber;
                address.Suite = foundGig.Address.Suite;
                type.Name = foundGig.GigType.Name;

                gig.Band = band;
                gig.Address = address;
                gig.GigType = type;
                gigsGet.Add(gig);
            }
            return Ok(gigsGet);
        }


        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "client, band")]
        public async Task<IActionResult> DeleteGigAsync(Guid id)
        {
            var isGigDeleted = await _gigService.DeleteGigAsync(id);
            if (!isGigDeleted)
            {
                return BadRequest("Gig not deleted");
            }
            return Ok("Gig deleted");
        }

        [HttpPut]
        [Route("confirm/{id}")]
        [Authorize(Roles ="band")]
        public async Task<IActionResult> ConfirmGigAsync(Guid id)
        {
            var isGigDeleted = await _gigService.ConfirmGigAsync(id);
            if (!isGigDeleted)
            {
                return BadRequest("Gig not deleted");
            }
            return Ok("Gig deleted");
        }
    }
}