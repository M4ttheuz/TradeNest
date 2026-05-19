using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TradeNest.Models
{
    public enum UserRole
    {
        User = 0,
        Admin = 1
    }
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Login { get; set; } = null!;
        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        public string Password { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;

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
