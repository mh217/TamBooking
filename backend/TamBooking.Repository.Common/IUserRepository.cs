using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);

        Task<User?> GetUserByIdAsync(Guid id);

        Task<Guid> InsertUserAsync(string email, string password, Guid roleId);

        Task<bool> ChangeUserPasswordAsync(Guid id, string password);

        Task<bool> ChangeUserEmailAsync(Guid id, string email);

        Task<bool> ActivateUser(Guid id);
    }
}