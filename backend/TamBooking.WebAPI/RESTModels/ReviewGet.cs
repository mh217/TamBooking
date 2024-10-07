namespace TamBooking.WebAPI.RESTModels
{
    public class ReviewGet
    {
        public Guid Id { get; set; }

        public int Rating { get; set; }

        public string Text { get; set; }

        public DateTime DateCreated { get; set; }
    }
}