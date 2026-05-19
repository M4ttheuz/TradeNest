using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using TradeNest.Models;
using TradeNest.Services;

namespace TradeNest.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserService _userService;
        private readonly PasswordHasher<User> _hasher;

        public AuthController(UserService userService)
        {
            _userService = userService;
            _hasher = new PasswordHasher<User>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        private bool IsPasswordStrong(string password)
        {
            var hasLower = Regex.IsMatch(password, @"[a-z]");
            var hasUpper = Regex.IsMatch(password, @"[A-Z]");
            var hasDigit = Regex.IsMatch(password, @"\d");
            var hasSpecial = Regex.IsMatch(password, @"[\W_]");
            var hasMinLength = password.Length >= 8;

            return hasLower && hasUpper && hasDigit && hasSpecial && hasMinLength;
        }

        [HttpPost]
        public IActionResult Register(string username, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
                return RedirectToAction("Login");

            if (_userService.UserExists(username))
                return RedirectToAction("Login");

            if (!IsPasswordStrong(password))
                return RedirectToAction("Login");

            var user = new User
            {
                Login = username,
                Email = email
            };

            user.Password = _hasher.HashPassword(user, password);

            _userService.Add(user);

            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _userService.GetByLogin(username);

            if (user == null)
                return RedirectToAction("Login");

            var result = _hasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Failed)
                return RedirectToAction("Login");

            if (!user.IsActive)
                return RedirectToAction("Login");

            HttpContext.Session.SetString("user", user.Login);
            HttpContext.Session.SetString("userId", user.Id.ToString());
            HttpContext.Session.SetString("role", user.Role.ToString());

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}