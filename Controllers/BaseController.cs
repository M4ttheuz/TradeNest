using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;
using TradeNest.Models;

namespace TradeNest.Controllers
{
    public class BaseController : Controller
    {
        protected bool EnsureAdmin()
        {
            var role = HttpContext.Session.GetString("role");

            return role == UserRole.Admin.ToString();
        }

        protected bool EnsureAdminOrOwner(int OwnerId)
        {
            var role = HttpContext.Session.GetString("role");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (role == UserRole.Admin.ToString())
                return true;

            if (userId != null && userId == OwnerId)
                return true;

            return false;
        }
    }
}
/*

* To jest kontroler bazowy który upewnia się że dana funkcja w kontrolerze może być wywołana tylko przez tych posiadających uprawnienia odpowiednie
** Jeżeli uzywacie to pamietajcie zeby dziedziczenie zmienic z xxxController : Controller na xxxController : BaseController
*** Tu macie jak je wywołać przyklad 

if (!EnsureAdmin())
    return RedirectToAction("Login", "Auth"); -- Wywali do logowania

if (!EnsureAdminOrOwner(listing.OwnerId))
    return RedirectToAction("Index", "Home"); -- Wywali do Strony głownej
*/

