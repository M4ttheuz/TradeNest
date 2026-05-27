namespace TradeNest.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string TelephoneNumber { get; set; } = null!;

        public string Role { get; set; } = "User";

        public bool IsActive { get; set; } = true;

        public ICollection<Listing> Listings { get; set; } = new List<Listing>();

        public ICollection<Listing> SavedListings { get; set; } = new List<Listing>();

        public ICollection<UserReview> ReceivedReviews { get; set; } = new List<UserReview>();
    }
}
