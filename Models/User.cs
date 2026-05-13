using System.ComponentModel.DataAnnotations.Schema;
namespace TradeNest.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = "User";

        public bool IsActive { get; set; } = true;

        // Opinie otrzymane
        public ICollection<UserReview> ReceivedReviews { get; set; } = new List<UserReview>();
    
        // Opinie napisane
        public ICollection<UserReview> WrittenReviews { get; set; } = new List<UserReview>();
    
    
        // Liczba opinii
        [NotMapped] // Bo nie ma kolumny w bazie danych i nie ma potrzeby jej tam dodawac
        public int ReviewsCount => ReceivedReviews.Count;
    
    
        // Średnia ocen (dopiero od 10 opinii)
        [NotMapped] // Bo nie ma kolumny w bazie danych i nie ma potrzeby jej tam dodawac
        public double? AverageRating =>
            ReviewsCount >= 10
                ? Math.Round(ReceivedReviews.Average(r => r.Rating), 2)
                : null;
        }
}
