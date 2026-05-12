namespace TradeNest.Models
{
    public enum ParameterType
    {
        Text = 0,
        Number = 1,
        Boolean = 2,
        Date = 3
    }

    public class CategoryParameter
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public string Name { get; set; } = null!;

        public ParameterType Type { get; set; }

        public bool IsRequired { get; set; }

        public ICollection<ListingParameterValue> Values { get; set; } = new List<ListingParameterValue>();
    }
}
