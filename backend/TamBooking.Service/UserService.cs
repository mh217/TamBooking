using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;
using System.Security.Claims;
using TamBooking.Common;
using TamBooking.Model;
using TamBooking.Model.DomainModels;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class UserService : IUserService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public UserService(IOptions<EmailSettings> emailSettings, IHttpContextAccessor httpContextAccessor,
            IJwtService jwtService, IUserRepository userRepository, IEmailService emailService)
        {
            _emailSettings = emailSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task<DomainAuthenticationResponse?> AuthenticateAsync(DomainAuthenticationRequest authenticationRequest)
        {
            User? user = await _userRepository.GetUserByEmailAsync(authenticationRequest.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(authenticationRequest.Password, user.Password))
            {
                return null;
            }

            UserClaims userClaims = new()
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Role = user.Role.Name.ToString(),
            };

            string token = await _jwtService.GenerateJwtToken(userClaims);

            if (!user.IsActive)
            {
                await SendConfirmationEmailAsync(authenticationRequest.Email, token);
                throw new InvalidOperationException("User is not activated.");
            }

            return new DomainAuthenticationResponse { Id = user.Id, Token = token };
        }

        public async Task<Guid> RegisterUserAsync(DomainRegistrationRequest registrationRequest)
        {
            User? user = await _userRepository.GetUserByEmailAsync(registrationRequest.Email);

            if (user != null)
            {
                throw new InvalidOperationException($"{registrationRequest.Email} already exists.");
            }

            Guid id = await _userRepository.InsertUserAsync(registrationRequest.Email,
                BCrypt.Net.BCrypt.HashPassword(registrationRequest.Password), registrationRequest.RoleId);

            user = await _userRepository.GetUserByIdAsync(id) ?? throw new Exception();
            UserClaims userClaims = new()
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Role = user.Role.Name,
            };
            string token = await _jwtService.GenerateJwtToken(userClaims);
            await SendConfirmationEmailAsync(registrationRequest.Email, token);
            return id;
        }

        public async Task<bool> ChangeUserPasswordAsync(DomainPasswordChangeRequest passwordChangeRequest)
        {
            if (passwordChangeRequest.NewPassword != passwordChangeRequest.ConfirmedPassword)
            {
                throw new InvalidOperationException("Passwords do not match.");
            }
            Guid id = Guid.Parse(_jwtService.GetCurrentUserClaims().Id);
            User? user = await _userRepository.GetUserByIdAsync(id);
            if (user == null || !BCrypt.Net.BCrypt.Verify(passwordChangeRequest.OldPassword, user.Password))
            {
                return false;
            }
            return await _userRepository.ChangeUserPasswordAsync(id, BCrypt.Net.BCrypt.HashPassword(passwordChangeRequest.NewPassword));
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> ChangeUserEmailAsync(string email)
        {
            Guid id = Guid.Parse(_jwtService.GetCurrentUserClaims().Id);
            return await _userRepository.ChangeUserEmailAsync(id, email);
        }

        public async Task<bool> ConfirmEmailAsync(string jwt)
        {
            ClaimsPrincipal claimsPrincipal = await _jwtService.ValidateTokenAsync(jwt);
            return await _userRepository.ActivateUser(Guid.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityTokenException("Invalid token")));
        }

        public async Task ResendConfirmationEmailAsync(string jwt)
        {
            UserClaims userClaims = _jwtService.ExtractUserClaims(jwt);
            var LogLevel = userClaims.Role;
            string newJwt = await _jwtService.GenerateJwtToken(userClaims);
            await SendConfirmationEmailAsync(userClaims.Email, newJwt);
        }

        public async Task SendConfirmationEmailAsync(string email, string token)
        {
            string endpointUrl = $"http://localhost:3000/confirmEmail?token={token}";
            var emailBody = $@"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Email confirmation</title>
                </head>
                <body style=""background-color: #fff9e3; padding: 20px"">
                    <div>
                        <h2>Greetings!</h2>
                        <p>Click the button below to confirm your e-mail for TamBooking website:</p>
                        <a href=""{endpointUrl}"" style=""
                            background-color: #872d03;
                            color: white;
                            padding: 10px 15px;
                            text-align: center;
                            text-decoration: none;
                            display: inline-block;
                            font-size: 16px;
                            cursor: pointer;
                            border-radius: 10px;"">
                            Confirm
                        </a>
                    </div>
                </body>
                </html>";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail),
                Subject = "Email confirmation",
                Body = emailBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);
            using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);

            smtpClient.EnableSsl = _emailSettings.EnableSsl;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}