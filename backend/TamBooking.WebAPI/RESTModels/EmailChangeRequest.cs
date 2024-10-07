using System.ComponentModel.DataAnnotations;

namespace TamBooking.WebAPI.RESTModels
{
    public class EmailChangeRequest
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
