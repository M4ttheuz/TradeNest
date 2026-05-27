using Microsoft.AspNetCore.Mvc;

namespace TradeNest.Controllers
{
    public class WatchlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}