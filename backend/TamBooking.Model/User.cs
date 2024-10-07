namespace TamBooking.Model
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; } = false;

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime DateUpdated { get; set; } = DateTime.Now;

        public Guid RoleId { get; set; }

        public Role Role { get; set; }
    }
}