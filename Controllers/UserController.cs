using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeNest.Data; 
using TradeNest.ViewModels; 

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
    }
}