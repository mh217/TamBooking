namespace TamBooking.WebAPI.RESTModels
{
    public class RESTAddressCreate
    {
        public string Line { get; set; }

        public string Suite { get; set; }

        public int BuildingNumber { get; set; }

        public Guid TownId { get; set; }
    }
}