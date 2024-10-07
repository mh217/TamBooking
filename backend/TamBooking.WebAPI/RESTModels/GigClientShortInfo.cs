using TamBooking.Model;

namespace TamBooking.WebAPI.RESTModels
{
    public class GigClientShortInfo
    {
        public Guid Id { get; set; }
        public DateTime OccasionDate { get; set; }

        public Guid BandId { get; set; }

        public Band Band { get; set; }
    }
}