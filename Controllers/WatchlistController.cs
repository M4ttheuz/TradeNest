using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TradeNest.Data;
using TradeNest.Models;

namespace TradeNest.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WatchlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string? userIdString = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Auth");
            }

            int userId = int.Parse(userIdString);

            var user = await _context.Users
                .Include(u => u.SavedListings)
                    .ThenInclude(l => l.Images)
                .Include(u => u.SavedListings)
                    .ThenInclude(l => l.Prices)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var savedListings = user?.SavedListings ?? new List<Listing>();

            Console.WriteLine("===== SAVED LISTINGS =====");
            Console.WriteLine($"Liczba zapisanych ogloszen: {savedListings.Count}");
            if (!savedListings.Any())
            {
                Console.WriteLine("BRAK ZAPISANYCH OGLOSZEN");
            }
            else
            {
                foreach (var listing in savedListings)
                {
                    Console.WriteLine($"[{listing.Id}] {listing.Title}");
                }
            }
            Console.WriteLine("==========================");

            return View(savedListings);
        }




        [HttpPost]
        public async Task<IActionResult> Toggle(int listingId)
        {
            string? userIdString = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Auth");
            }

            int userId = int.Parse(userIdString);

            var user = await _context.Users
                .Include(u => u.SavedListings)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound();

            var listing = await _context.Listings
                .FirstOrDefaultAsync(l => l.Id == listingId);

            if (listing == null)
                return NotFound();

            if (user.SavedListings.Any(x => x.Id == listingId))
            {
                user.SavedListings.Remove(listing);

                Console.WriteLine($"USUNIETO Z WATCHLISTY: {listing.Title}");
            }
            else
            {
                user.SavedListings.Add(listing);

                Console.WriteLine($"DODANO DO WATCHLISTY: {listing.Title}");
            }

            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }


    }
}