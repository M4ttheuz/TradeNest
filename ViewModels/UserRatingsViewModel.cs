using TradeNest.Models;

namespace TradeNest.ViewModels
{
    public class UserRatingsViewModel
    {
        public User User { get; set; } = null!;
        public IEnumerable<UserReview> Reviews { get; set; } = new List<UserReview>();
        public string CurrentSort { get; set; } = "Najnowsze";
    }
}