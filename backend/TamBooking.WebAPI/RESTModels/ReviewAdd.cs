namespace TamBooking.WebAPI.RESTModels
{
    public class ReviewAdd
    {
        public int Rating { get; set; }

        public string Text { get; set; }

        public Guid BandId { get; set; }
    }
}