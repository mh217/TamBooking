namespace TamBooking.Model
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public bool IsActive { get; set; }
    }
}