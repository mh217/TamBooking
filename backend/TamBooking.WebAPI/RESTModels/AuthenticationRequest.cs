using System.ComponentModel.DataAnnotations;

namespace TamBooking.WebAPI.RESTModels
{
    public class AuthenticationRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8)]
        public string Password { get; set; }
    }
}