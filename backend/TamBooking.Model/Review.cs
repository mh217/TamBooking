namespace TamBooking.Model
{
    public class Review
    {
        public Guid Id { get; set; }

        public int Rating { get; set; }

        public string Text { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime DateUpdated { get; set; } = DateTime.Now;

        public Guid CreatedByUserId { get; set; }

        public Guid UpdatedByUserId { get; set; }

        public Guid ClientId { get; set; }

        public Guid BandId { get; set; }

        public Client? Client { get; set; }

        public Band? Band { get; set; }
    }
}