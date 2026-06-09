namespace TradeNest.Models
{
    public class Listing
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Location { get; set; } = null!;

        public bool IsVisible { get; set; } = true;

        public bool IsApproved { get; set; } = false;

        public bool IsPromoted { get; set; } = false;
        
        public DateTime PromotionEndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<ListingPrice> Prices { get; set; } = new List<ListingPrice>();

        public ICollection<ListingParameterValue> ParameterValues { get; set; } = new List<ListingParameterValue>();

        public ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();
    }
}
