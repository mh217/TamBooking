using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleByIdAsync(Guid id);

        Task<List<Role>> GetRolesAsync();
    }
}
