namespace TamBooking.Common
{
    public class BandFilter
    {
        public Guid? Id { get; set; }
        public string SearchQuery { get; set; }

        public decimal PriceFrom { get; set; }

        public decimal PriceTo { get; set; }

        public Guid? CountyId { get; set; }
    }
}