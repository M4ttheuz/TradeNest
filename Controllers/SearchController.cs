using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TradeNest.Data;
using TradeNest.Models;

public class SearchController : Controller
{
    private readonly ApplicationDbContext _db;

    public SearchController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(
        string? phrase,
        int? categoryId,
        double? minPrice,
        double? maxPrice,
        Dictionary<string, string> filters,
        SortBy sortBy = SortBy.Date,
        SortDirection sortDirection = SortDirection.Descending)
    {
        IQueryable<Listing> q = _db.Listings
            .Where(l => l.IsVisible && l.IsApproved);

        if (!string.IsNullOrWhiteSpace(phrase))
        {
            var p = phrase.Trim().ToLower();
            q = q.Where(l =>
                l.Title.ToLower().Contains(p) ||
                l.Description.ToLower().Contains(p) ||
                l.Location.ToLower().Contains(p));
        }

        if (categoryId.HasValue)
            q = q.Where(l => l.CategoryId == categoryId.Value);

        if (minPrice.HasValue)
            q = q.Where(l => l.Prices
                .OrderByDescending(p => p.SetAt)
                .First().Price >= minPrice.Value);

        if (maxPrice.HasValue)
            q = q.Where(l => l.Prices
                .OrderByDescending(p => p.SetAt)
                .First().Price <= maxPrice.Value);

        if (filters.Any())
        {
            var paramIds = filters.Keys
                .Select(k => k.Replace("_min", "").Replace("_max", ""))
                .Select(k => int.TryParse(k, out var id) ? id : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            var parameters = await _db.CategoryParameters
    .Where(p => paramIds.Contains(p.Id))
    .AsNoTracking()
    .ToListAsync();

            // TEMP DEBUG
            Console.WriteLine($"[DEBUG] filters.Count={filters.Count}");
            foreach (var f in filters)
                Console.WriteLine($"[DEBUG] filter key='{f.Key}', value='{f.Value}'");

            Console.WriteLine($"[DEBUG] paramIds={string.Join(",", paramIds)}");
            Console.WriteLine($"[DEBUG] parameters.Count={parameters.Count}");
            foreach (var p in parameters)
                Console.WriteLine($"[DEBUG] param id={p.Id}, name={p.Name}, type={p.Type}");

            foreach (var param in parameters)
            {
                var id = param.Id;

                var hasMin = filters.TryGetValue($"{id}_min", out var minRaw) && !string.IsNullOrWhiteSpace(minRaw);
                var hasMax = filters.TryGetValue($"{id}_max", out var maxRaw) && !string.IsNullOrWhiteSpace(maxRaw);
                double.TryParse(minRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out var minNum);
                double.TryParse(maxRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out var maxNum);

                filters.TryGetValue(id.ToString(), out var rawValue);

                switch (param.Type)
                {
                    case ParameterType.Text:
                        if (string.IsNullOrWhiteSpace(rawValue)) break;
                        var text = rawValue.ToLower();
                        q = q.Where(l => l.ParameterValues.Any(pv =>
                            pv.CategoryParameterId == id &&
                            pv.Value.ToLower().Contains(text)));
                        break;

                    case ParameterType.Number:
                        var debugValues = await _db.ListingParameterValues
                            .Where(pv => pv.CategoryParameterId == id)
                            .AsNoTracking()
                            .ToListAsync();

                        foreach (var dv in debugValues)
                            Console.WriteLine($"[DEBUG] ListingId={dv.ListingId}, Value='{dv.Value}'");

                        Console.WriteLine($"[DEBUG] hasMin={hasMin}, minRaw='{minRaw}', minNum={minNum}");
                        Console.WriteLine($"[DEBUG] hasMax={hasMax}, maxRaw='{maxRaw}', maxNum={maxNum}");

                        if (!hasMin && !hasMax) break;
                        var matchingIds = await _db.ListingParameterValues
                            .Where(pv => pv.CategoryParameterId == id)
                            .AsNoTracking()
                            .ToListAsync()
                            .ContinueWith(t => t.Result
                                .Where(pv => double.TryParse(pv.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) &&
                                    (!hasMin || v >= minNum) &&
                                    (!hasMax || v <= maxNum))
                                .Select(pv => pv.ListingId)
                                .ToHashSet());
                        q = q.Where(l => matchingIds.Contains(l.Id));
                        break;

                    case ParameterType.Boolean:
                        if (string.IsNullOrWhiteSpace(rawValue)) break;
                        q = q.Where(l => l.ParameterValues.Any(pv =>
                            pv.CategoryParameterId == id &&
                            pv.Value.ToLower() == rawValue.ToLower()));
                        break;

                    case ParameterType.Date:
                        if (string.IsNullOrWhiteSpace(rawValue)) break;
                        q = q.Where(l => l.ParameterValues.Any(pv =>
                            pv.CategoryParameterId == id &&
                            pv.Value == rawValue));
                        break;
                }
            }
        }

        q = (sortBy, sortDirection) switch
        {
            (SortBy.Price, SortDirection.Ascending) =>
                q.OrderBy(l => l.Prices.OrderByDescending(p => p.SetAt).Select(p => p.Price).FirstOrDefault()),
            (SortBy.Price, SortDirection.Descending) =>
                q.OrderByDescending(l => l.Prices.OrderByDescending(p => p.SetAt).Select(p => p.Price).FirstOrDefault()),
            (SortBy.Date, SortDirection.Ascending) =>
                q.OrderBy(l => l.CreatedAt),
            _ =>
                q.OrderByDescending(l => l.CreatedAt),
        };

        var listings = await q
            .Include(l => l.Category)
                .ThenInclude(c => c.Parameters)
            .Include(l => l.Prices)
            .Include(l => l.Images)
            .AsNoTracking()
            .ToListAsync();

        ViewBag.Phrase = phrase;
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.MinPrice = minPrice;
        ViewBag.MaxPrice = maxPrice;
        ViewBag.SortBy = sortBy;
        ViewBag.SortDirection = sortDirection;
        ViewBag.Categories = await _db.Categories
            .Include(c => c.Parameters)
            .AsNoTracking()
            .ToListAsync();

        return View(listings);
    }
}

public enum SortBy { Date, Price }
public enum SortDirection { Ascending, Descending }