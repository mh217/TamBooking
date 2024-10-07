using TamBooking.Model;

namespace TamBooking.WebAPI.RESTModels
{
    public class GigBandShortInfo
    {
        public Guid Id { get; set; }
        public DateTime OccasionDate { get; set; }

        public Guid ClientId { get; set; }

        public Client Client { get; set; }
    }
}