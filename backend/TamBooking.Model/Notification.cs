namespace TamBooking.Model
{
    public class Notification
    {
        public Guid Id { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime DateUpdated { get; set; } = DateTime.Now;

        public Guid CreatedByUserId { get; set; }

        public Guid UpdatedByUserId { get; set; }

        public Guid RecepientTypeId { get; set; }

        public Guid ClientId { get; set; }

        public Guid BandId { get; set; }
        public Guid GigId { get; set; }

        public RecepientType RecepientType { get; set; }

        public Client Client { get; set; }

        public Band Band { get; set; }
        public Gig Gig { get; set; }
    }
}