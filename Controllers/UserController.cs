using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeNest.Data; 
using TradeNest.ViewModels;
using TradeNest.Models;

namespace TradeNest.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> UserRatings(int id, string sortBy = "Najnowsze")
        { 
            var user = await _context.Users
                .Include(u => u.ReceivedReviews)
                .FirstOrDefaultAsync(u => u.Id == id);
            
            if (user == null)
            {
                return NotFound();
            }

            
            var reviewsQuery = _context.UserReviews
                .Where(r => r.TargetUserId == id)
                .Include(r => r.Author)  
                .Include(r => r.Listing) 
                .AsQueryable();

            
            reviewsQuery = sortBy switch
            {
                "Najwyższe oceny" => reviewsQuery.OrderByDescending(r => r.DescriptionRating + r.ResponseTimeRating + r.PolitenessRating),
               
                "Najniższe oceny" => reviewsQuery.OrderBy(r => r.DescriptionRating + r.ResponseTimeRating + r.PolitenessRating),

                _ => reviewsQuery.OrderByDescending(r => r.CreatedAt)
            };

            var viewModel = new UserRatingsViewModel
            {
                User = user,
                Reviews = await reviewsQuery.ToListAsync(), 
                CurrentSort = sortBy
            };

            ViewData["Title"] = $"Oceny użytkownika {user.Login}";

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateReview(int targetUserId, int? listingId)
        {
            var currentUserIdStr = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(currentUserIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            if (currentUserIdStr == targetUserId.ToString())
            {
                return RedirectToAction("UserRatings", new { id = targetUserId });
            }

            ViewBag.TargetUserId = targetUserId;
            ViewBag.ListingId = listingId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReview(UserReview review)
        {
            var currentUserIdStr = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(currentUserIdStr) || !int.TryParse(currentUserIdStr, out int currentUserId))
            {
                return RedirectToAction("Login", "Account");
            }

            review.AuthorId = currentUserId;
            review.CreatedAt = DateTime.Now;

            ModelState.Remove(nameof(review.Author));
            ModelState.Remove(nameof(review.TargetUser));

            if (ModelState.IsValid)
            {
                _context.UserReviews.Add(review);
                await _context.SaveChangesAsync();

                return RedirectToAction("UserRatings", new { id = review.TargetUserId });
            }

            ViewBag.TargetUserId = review.TargetUserId;
            ViewBag.ListingId = review.ListingId;

            return View(review);
        }

    }
}