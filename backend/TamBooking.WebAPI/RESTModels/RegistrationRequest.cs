using System.ComponentModel.DataAnnotations;

namespace TamBooking.WebAPI.RESTModels
{
    public class RegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }
}