
using TamBooking.Model;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _roleRepository.GetRolesAsync();
        }
    }
}
