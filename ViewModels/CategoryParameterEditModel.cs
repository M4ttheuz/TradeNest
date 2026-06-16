using TradeNest.Models;

namespace TradeNest.ViewModels
{
    public class CategoryParameterEditModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ParameterType Type { get; set; }
        public bool IsRequired { get; set; }
    }
}
