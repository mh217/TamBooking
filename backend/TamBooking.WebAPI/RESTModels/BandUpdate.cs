namespace TamBooking.WebAPI.RESTModels
{
    public class BandUpdate
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public Guid TownId { get; set; }
    }
}