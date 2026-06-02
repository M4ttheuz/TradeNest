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
            int userId = 1; // TEMP DEBUG USER

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
    }
}