namespace TamBooking.Model
{
    public class Client
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime DateUpdated { get; set; } = DateTime.Now;

        public Guid TownId { get; set; }

        public User User { get; set; }

        public Town Town { get; set; }
    }
}