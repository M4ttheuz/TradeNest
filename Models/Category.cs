namespace TradeNest.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<CategoryParameter> Parameters { get; set; } = new List<CategoryParameter>();

        public ICollection<Listing> Listings { get; set; } = new List<Listing>();
    }
}
