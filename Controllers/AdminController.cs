using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TradeNest.Data;
using TradeNest.Services;

namespace TradeNest.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!EnsureAdmin())
                return RedirectToAction("Login", "Auth");

            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.ListingsCount = _context.Listings.Count();
            ViewBag.CategoriesCount = _context.Categories.Count();

            return View();
        }

        public IActionResult Users()
        {
            if (!EnsureAdmin())
                return RedirectToAction("Login", "Auth");

            var users = _context.Users.ToList();
            return View(users); 
        }

        public IActionResult Listings()
        {
           if (!EnsureAdmin())
                return RedirectToAction("Login", "Auth");

            var listings = _context.Listings.ToList();
            return View(listings);
        }

        public IActionResult Categories()
        {
            if (!EnsureAdmin())
                return RedirectToAction("Login", "Auth");

            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [HttpPost]
        public IActionResult ToggleUserStatus(int userId)
        {
            if (!EnsureAdmin())
                return RedirectToAction("Login", "Auth");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                user.IsActive = !user.IsActive;
                _context.SaveChanges();
            }

            return RedirectToAction("Users");
        }
    }
}