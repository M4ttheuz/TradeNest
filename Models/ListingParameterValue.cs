namespace TradeNest.Models
{
    public class ListingParameterValue
    {
        public int Id { get; set; }

        public int ListingId { get; set; }
        public Listing Listing { get; set; } = null!;

        public int CategoryParameterId { get; set; }
        public CategoryParameter CategoryParameter { get; set; } = null!;

        public string Value { get; set; } = null!;
    }
}
