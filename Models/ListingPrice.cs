namespace TradeNest.Models
{
    public class ListingPrice
    {
        public int Id { get; set; }

        public int ListingId { get; set; }
        public Listing Listing { get; set; } = null!;

        public double Price { get; set; }

        public DateTime SetAt { get; set; }
    }
}
