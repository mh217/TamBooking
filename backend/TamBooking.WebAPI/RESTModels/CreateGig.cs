namespace TamBooking.WebAPI.RESTModels
{
    public class CreateGig
    {
        public DateTime OccasionDate { get; set; }

        public Guid TypeId { get; set; }

        public Guid AddressId { get; set; }

        public Guid BandId { get; set; }
    }
}