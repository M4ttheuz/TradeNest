using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TradeNest.Data;
using TradeNest.Models;

namespace TradeNest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            List<Listing> listings = _context.Listings
                .Where(x => x.IsVisible)
                .Include(x => x.Category)
                .Include(x => x.Prices)
                .Include(x => x.Images)
                .Include(x => x.ParameterValues)
                .ThenInclude(x => x.CategoryParameter)
                .OrderByDescending(x => x.CreatedAt)
                .Take(12)
                .ToList();

            ViewBag.User = HttpContext.Session.GetString("user");
            ViewBag.UserId = HttpContext.Session.GetString("userId");
            ViewBag.Role = HttpContext.Session.GetString("role");

            return View(listings);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
