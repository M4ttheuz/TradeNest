using Microsoft.AspNetCore.Mvc;
using TradeNest.Data;
using System.Linq;

namespace TradeNest.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.ListingsCount = _context.Listings.Count();
            ViewBag.CategoriesCount = _context.Categories.Count();

            return View();
        }

        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Listings()
        {
            var listings = _context.Listings.ToList();
            return View(listings);
        }

        public IActionResult Categories()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
    }
}