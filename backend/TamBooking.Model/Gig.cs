namespace TamBooking.Model
{
    public class Gig
    {
        public Guid Id { get; set; }

        public DateTime OccasionDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime DateUpdated { get; set; } = DateTime.Now;

        public Guid CreatedByUserId { get; set; }

        public Guid UpdatedByUserId { get; set; }

        public Guid TypeId { get; set; }

        public Guid AddressId { get; set; }

        public Guid ClientId { get; set; }

        public Guid BandId { get; set; }

        public GigType GigType { get; set; }

        public Address Address { get; set; }

        public Client Client { get; set; }

        public Band Band { get; set; }
    }
}