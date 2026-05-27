namespace TradeNest.Models
{
    public class ListingImage
    {
        public int Id { get; set; }

        public string ImagePath { get; set; } = null!;

        public bool IsMain { get; set; } = false;

        public int ListingId { get; set; }
        public Listing Listing { get; set; } = null!;
    }
}
