namespace TamBooking.Model.DomainModels
{
    public class DomainAddressCreate
    {
        public string Line { get; set; }

        public string Suite { get; set; }

        public int BuildingNumber { get; set; }

        public Guid TownId { get; set; }
    }
}