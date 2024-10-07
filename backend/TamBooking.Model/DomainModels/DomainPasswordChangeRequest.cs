namespace TamBooking.Model.DomainModels
{
    public class DomainPasswordChangeRequest
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmedPassword { get; set; }
    }
}