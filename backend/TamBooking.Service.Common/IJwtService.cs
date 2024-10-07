using System.Security.Claims;
using TamBooking.Model.DomainModels;

namespace TamBooking.Service.Common
{
    public interface IJwtService
    {
        Task<string> GenerateJwtToken(UserClaims user);

        Task<ClaimsPrincipal> ValidateTokenAsync(string token);

        UserClaims ExtractUserClaims(string token);

        UserClaims GetCurrentUserClaims();
    }
}