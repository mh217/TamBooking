using TamBooking.Model;

namespace TamBooking.WebAPI.RESTModels
{
    public class GigBandMoreInfo
    {
        public Guid Id { get; set; }
        public DateTime OccasionDate { get; set; }

        public GigType GigType { get; set; }

        public Address Address { get; set; }

        public Client Client { get; set; }
    }
}