using TamBooking.Model;

namespace TamBooking.WebAPI.RESTModels
{
    public class ReviewWithBandGet
    {
        public Guid Id { get; set; }

        public int Rating { get; set; }

        public string Text { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid BandId { get; set; }

        public string BandName { get; set; }
    }
}
