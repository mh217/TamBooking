namespace TamBooking.WebAPI.RESTModels
{
    public class GetNotification
    {
        public Guid Id { get; set; }

        public Guid GigId { get; set; }

        public string From { get; set; }
        public string To { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
    }
}