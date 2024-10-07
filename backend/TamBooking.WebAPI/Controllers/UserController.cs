using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TamBooking.Model.DomainModels;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate(AuthenticationRequest authenticationRequest)
        {
            DomainAuthenticationRequest domainAuthenticationRequest = new()
            {
                Email = authenticationRequest.Email,
                Password = authenticationRequest.Password,
            };
            var response = await _userService.AuthenticateAsync(domainAuthenticationRequest);

            if (response == null)
                return Unauthorized("Username or password is incorrect.");

            return Ok(new AuthenticationResponse { Id = response.Id, Token = response.Token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
        {
            DomainRegistrationRequest domainRegistrationRequest = new()
            {
                Email = registrationRequest.Email,
                Password = registrationRequest.Password,
                RoleId = registrationRequest.RoleId,
            };
            await _userService.RegisterUserAsync(domainRegistrationRequest);

            return StatusCode(201);
        }

        [HttpPut("change_password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(PasswordChangeRequest passwordChangeRequest)
        {
            DomainPasswordChangeRequest domainPasswordChangeRequest = new()
            {
                OldPassword = passwordChangeRequest.OldPassword,
                NewPassword = passwordChangeRequest.NewPassword,
                ConfirmedPassword = passwordChangeRequest.ConfirmedPassword
            };
            var isChanged = await _userService.ChangeUserPasswordAsync(domainPasswordChangeRequest);

            if (isChanged == false)
                return BadRequest("Invalid username or password");

            return NoContent();
        }

        [HttpPut("change_email")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(EmailChangeRequest emailChangeRequest)
        {
            var isChanged = await _userService.ChangeUserEmailAsync(emailChangeRequest.Email);
            if (isChanged == false)
                return BadRequest("User not found");

            return NoContent();
        }

        [HttpGet("confirm_email/{token}")]
        public async Task<IActionResult> ConfirmEmailAsync(string token)
        {
            bool isChanged = await _userService.ConfirmEmailAsync(token);
            if (isChanged == false)
                return BadRequest("User not found");

            return NoContent();
        }

        [HttpGet("resend_confirmation_email/{token}")]
        public async Task<IActionResult> ResendConfirmationEmailAsync(string token)
        {
            await _userService.ResendConfirmationEmailAsync(token);
            return NoContent();
        }
    }
}