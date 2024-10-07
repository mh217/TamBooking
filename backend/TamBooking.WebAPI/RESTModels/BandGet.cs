namespace TamBooking.WebAPI.RESTModels
{
    public class BandGet
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public Guid TownId { get; set; }
    }
}