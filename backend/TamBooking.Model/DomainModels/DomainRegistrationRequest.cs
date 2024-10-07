namespace TamBooking.Model.DomainModels
{
    public class DomainRegistrationRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public Guid RoleId { get; set; }
    }
}