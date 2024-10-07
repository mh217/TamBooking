using TamBooking.Model;

namespace TamBooking.WebAPI.RESTModels
{
    public class GetClient
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public County County { get; set; }

        public Town Town { get; set; }
    }
}