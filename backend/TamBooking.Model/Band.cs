namespace TamBooking.Model
{
    public class Band
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public Guid TownId { get; set; }

        public User User { get; set; }

        public Town Town { get; set; }
    }
}