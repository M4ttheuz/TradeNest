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
        public int Id { get; set; }

        public string Login { get; set; } = null!;

        public string Email { get; set; } = null!;  

        public string Password { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string TelephoneNumber { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;

        public bool IsActive { get; set; } = true;

        public ICollection<Listing> Listings { get; set; } = new List<Listing>();

        public ICollection<Listing> SavedListings { get; set; } = new List<Listing>();

        public ICollection<UserReview> ReceivedReviews { get; set; } = new List<UserReview>();

        // Opinie napisane
        public ICollection<UserReview> WrittenReviews { get; set; } = new List<UserReview>();


        // Liczba opinii
        [NotMapped] // Bo nie ma kolumny w bazie danych i nie ma potrzeby jej tam dodawac
        public int ReviewsCount => ReceivedReviews.Count;


        // Średnia ogólna ocen (wyliczana ze wszystkich składowych)
        [NotMapped]
        public double? AverageRating =>
            ReviewsCount >= 1
                ? Math.Round(ReceivedReviews.Average(r => (r.DescriptionRating + r.ResponseTimeRating + r.PolitenessRating) / 3.0), 2)
                : null;

        // Średnia: Zgodność z opisem 
        [NotMapped]
        public double AverageDescriptionRating =>
            ReceivedReviews.Any() ? Math.Round(ReceivedReviews.Average(r => r.DescriptionRating), 1) : 0;

        // Średnia: Czas odpowiedzi 
        [NotMapped]
        public double AverageResponseTimeRating =>
            ReceivedReviews.Any() ? Math.Round(ReceivedReviews.Average(r => r.ResponseTimeRating), 1) : 0;

        // Średnia: Uprzejmość 
        [NotMapped]
        public double AveragePolitenessRating =>
            ReceivedReviews.Any() ? Math.Round(ReceivedReviews.Average(r => r.PolitenessRating), 1) : 0;
    }
}

