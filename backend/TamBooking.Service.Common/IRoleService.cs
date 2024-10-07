using TamBooking.Model;

namespace TamBooking.Service.Common
{
    public interface IRoleService
    {
        Task<List<Role>> GetRolesAsync();
    }
}
