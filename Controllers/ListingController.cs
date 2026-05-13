using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeNest.Data;
using TradeNest.Models;

namespace TradeNest.Controllers
{
    public class ListingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();

            return View();
        }

        [HttpGet]
        public IActionResult GetCategoryParameters(int categoryId)
        {
            List<CategoryParameter> parameters = _context.CategoryParameters
                .Where(x => x.CategoryId == categoryId)
                .ToList();

            return PartialView("_CategoryParameters", parameters);
        }

        [HttpPost]
        public IActionResult Create(
            string title,
            string description,
            double price,
            int categoryId,
            List<ParameterInputModel> parameters)
        {
            Listing listing = new()
            {
                Title = title,
                Description = description,
                CategoryId = categoryId,

                OwnerId = 1,

                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,

                IsVisible = true,
                IsApproved = true
            };

            listing.Prices.Add(new ListingPrice
            {
                Price = price,
                SetAt = DateTime.Now
            });

            foreach (var p in parameters)
            {
                listing.ParameterValues.Add(new ListingParameterValue
                {
                    CategoryParameterId = p.Id,
                    Value = p.Value
                });
            }

            _context.Listings.Add(listing);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            List<Listing> listings = _context.Listings
                .Where(x => x.IsVisible)
                .Include(x => x.Category)
                .Include(x => x.Prices)
                .Include(x => x.ParameterValues)
                .ThenInclude(x => x.CategoryParameter)
                .ToList();

            return View(listings);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Listing? listing = _context.Listings.FirstOrDefault(x => x.Id == id);

            if (listing == null) return RedirectToAction("Index");

            listing.IsVisible = false;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Listing? listing = _context.Listings
                .Include(x => x.Category)
                .Include(x => x.Category.Parameters)
                .Include(x => x.ParameterValues)
                .ThenInclude(x => x.CategoryParameter)
                .Include(x => x.Prices)
                .FirstOrDefault(x => x.Id == id);

            if (listing == null) return RedirectToAction("Index");

            return View(listing);
        }

        [HttpPost]
        public IActionResult Edit(
            int id,
            string title,
            string description,
            double price,
            List<int> parameterValueIds,
            List<string> parameterValues)
        {
            Listing? listing = _context.Listings
                .Include(x => x.ParameterValues)
                .Include(x => x.Prices)
                .FirstOrDefault(x => x.Id == id);

            if (listing == null) return RedirectToAction("Index");

            listing.Title = title;

            listing.Description = description;

            listing.UpdatedAt = DateTime.Now;

            listing.Prices.Add(new ListingPrice
            {
                Price = price,
                SetAt = DateTime.Now
            });

            for (int i = 0; i < parameterValueIds.Count; i++)
            {
                ListingParameterValue? parameter = listing.ParameterValues
                    .FirstOrDefault(x => x.Id == parameterValueIds[i]);

                if (parameter == null) continue;

                parameter.Value = parameterValues[i];
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}