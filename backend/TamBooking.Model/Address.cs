namespace TamBooking.Model
{
    public class Address
    {
        public Guid Id { get; set; }

        public string Line { get; set; }

        public string Suite { get; set; }

        public int BuildingNumber { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime DateUpdated { get; set; } = DateTime.Now;

        public Guid CreatedByUserId { get; set; }

        public Guid UpdatedByUserId { get; set; }

        public bool IsActive { get; set; }

        public Guid TownId { get; set; }

        public Town? Town { get; set; }
    }
}