using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeNest.Data;
using TradeNest.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TradeNest.Controllers
{
    public class ListingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ListingController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> Create(
            string title,
            string description,
            double price,
            string location,
            int categoryId,
            List<ParameterInputModel> parameters,
            IFormFile mainImage,     
            List<IFormFile> additionalImages)
        {
            Listing listing = new()
            {
                Title = title,
                Description = description,
                CategoryId = categoryId,
                Location = location,
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

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    listing.ParameterValues.Add(new ListingParameterValue
                    {
                        CategoryParameterId = p.Id,
                        Value = p.Value
                    });
                }
            }

            // Image upload handling
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            if (mainImage != null && mainImage.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(mainImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await mainImage.CopyToAsync(fileStream);
                }

                listing.Images.Add(new ListingImage
                {
                    ImagePath = "/images/" + uniqueFileName,
                    IsMain = true
                });
            }

            if (additionalImages != null && additionalImages.Count > 0)
            {
                foreach (var image in additionalImages)
                {
                    if (image.Length > 0)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        listing.Images.Add(new ListingImage
                        {
                            ImagePath = "/images/" + uniqueFileName,
                            IsMain = false
                        });
                    }
                }
            }

            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();

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
                .Include(x => x.Images)
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
                .Include(x => x.Images) 
                .FirstOrDefault(x => x.Id == id);

            if (listing == null) return RedirectToAction("Index");
            return View(listing);
        }

        [HttpPost]
        public IActionResult Edit(int id, string title, string description, double price, List<int> parameterValueIds, List<string> parameterValues)
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