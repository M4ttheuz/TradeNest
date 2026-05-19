using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TradeNest.Models;

namespace TradeNest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.User = HttpContext.Session.GetString("user");
            ViewBag.UserId = HttpContext.Session.GetString("userId");
            ViewBag.Role = HttpContext.Session.GetString("role");

            return View();
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
