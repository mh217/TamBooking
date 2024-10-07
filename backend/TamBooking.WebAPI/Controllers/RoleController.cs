using Microsoft.AspNetCore.Mvc;
using TamBooking.Model;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRolesAsync()
        {
            List<Role> roles = await _roleService.GetRolesAsync();
            return Ok(
                roles.Select(role => new RoleGet
                {
                    Id = role.Id,
                    Name = role.Name
                }).ToList());
        }
    }
}
