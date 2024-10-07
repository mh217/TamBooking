using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TamBooking.Model.DomainModels;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public JwtService(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<string> GenerateJwtToken(UserClaims user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {
                var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity([
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role),
                    ]),
                    Expires = DateTime.UtcNow.AddHours(8),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            return await Task.Run(() =>
            {
                var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                return principal;
            });
        }

        public UserClaims ExtractUserClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            UserClaims userClaims = new()
            {
                Id = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value ?? "",
                Email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value ?? "",
                Role = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role")?.Value ?? ""
            };
            return userClaims;
        }

        public UserClaims GetCurrentUserClaims()
        {
            UserClaims userClaims = new()
            {
                Id = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
                Email = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value ?? "",
                Role = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? ""
            };
            return userClaims;
        }
    }
}