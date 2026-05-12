using Microsoft.EntityFrameworkCore;
using TradeNest.Models;

namespace TradeNest.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryParameter> CategoryParameters { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<ListingPrice> ListingPrices { get; set; }
        public DbSet<ListingParameterValue> ListingParameterValues { get; set; }
        public DbSet<UserReview> UserReviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserReview>()
                .HasOne(r => r.TargetUser)
                .WithMany(u => u.ReceivedReviews)
                .HasForeignKey(r => r.TargetUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
