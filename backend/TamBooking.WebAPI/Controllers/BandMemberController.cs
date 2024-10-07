using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TamBooking.Model;
using TamBooking.Service;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [Route("api/bandMembers")]
    [ApiController]
    public class BandMemberController : ControllerBase
    {
        private readonly IBandMemberService _memberService;

        public BandMemberController(IBandMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpPost]
        [Authorize(Roles = "band")]
        public async Task<IActionResult> CreateBandMemberAsync(BandMemberInfo bandMemberInfo)
        {
            BandMember member = new()
            {
                FirstName = bandMemberInfo.FirstName,
                LastName = bandMemberInfo.LastName,
                Email = bandMemberInfo.Email
            };
            await _memberService.CreateBandMemeberAsync(member);
            return Created();
        }

        [HttpGet("{bandId}")]
        public async Task<IActionResult> GetBandMembersAsync(Guid bandId)
        {

            return Ok(await _memberService.GetBandMemberAsync(bandId));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "band")]
        public async Task<IActionResult> DeleteBandMemberAsync(Guid id)
        {
            var isBandMemberDeleted = await _memberService.DeleteBandMemberAsync(id);
            if (!isBandMemberDeleted)
            {
                return BadRequest("Band member not deleted");
            }
            return Ok("Band member deleted");
        }
    }
}