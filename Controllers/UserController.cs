using Microsoft.AspNetCore.Mvc;

namespace TradeNest.Controllers
{
    public class UserController : Controller
    {
        public IActionResult UserRatings()
        {
            ViewData["Title"] = "Oceny użytkownika";
            return View();
        }
    }
}
