namespace TamBooking.Model
{
    public class Town
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Zip { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime DateUpdated { get; set; } = DateTime.Now;

        public Guid CreatedByUserId { get; set; }

        public Guid UpdatedByUserId { get; set; }

        public Guid CountyId { get; set; }

        public County County { get; set; }
    }
}