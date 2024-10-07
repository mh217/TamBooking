using TamBooking.Model;
using TamBooking.Model.DomainModels;

namespace TamBooking.Service.Common
{
    public interface IUserService
    {
        Task<DomainAuthenticationResponse?> AuthenticateAsync(DomainAuthenticationRequest model);

        Task<Guid> RegisterUserAsync(DomainRegistrationRequest registrationRequest);

        Task<User?> GetUserByIdAsync(Guid id);

        Task<bool> ChangeUserPasswordAsync(DomainPasswordChangeRequest passwordChangeRequest);

        Task<bool> ChangeUserEmailAsync(string email);

        Task<bool> ConfirmEmailAsync(string jwt);

        Task ResendConfirmationEmailAsync(string jwt);

        Task SendConfirmationEmailAsync(string email, string token);
    }
}