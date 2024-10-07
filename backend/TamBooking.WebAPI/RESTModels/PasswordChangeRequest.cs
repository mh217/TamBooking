using System.ComponentModel.DataAnnotations;

namespace TamBooking.WebAPI.RESTModels
{
    public class PasswordChangeRequest
    {
        public string OldPassword { get; set; }

        [MinLength(8)]
        public string NewPassword { get; set; }

        [MinLength(8)]
        public string ConfirmedPassword { get; set; }
    }
}