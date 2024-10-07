using TamBooking.Model;

namespace TamBooking.WebAPI.RESTModels
{
    public class GigClientMoreInfo
    {
        public Guid Id { get; set; }
        public DateTime OccasionDate { get; set; }

        public GigType GigType { get; set; }

        public Address Address { get; set; }

        public Band Band { get; set; }
    }
}