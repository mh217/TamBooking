namespace TamBooking.WebAPI.RESTModels
{
    public class UpdateClient
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public Guid TownId { get; set; }
    }
}