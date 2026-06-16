using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeNest.Data;
using TradeNest.Models;

namespace TradeNest.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string categoryName,
            List<string> parameterNames,
            List<ParameterType> parameterTypes,
            List<bool> parameterRequired)
        {
            Category category = new()
            {
                Name = categoryName
            };

            for (int i = 0; i < parameterNames.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(parameterNames[i])) continue;

                CategoryParameter parameter = new()
                {
                    Name = parameterNames[i],
                    Type = parameterTypes[i],
                    IsRequired = parameterRequired[i]
                };

                category.Parameters.Add(parameter);
            }

            _context.Categories.Add(category);

            _context.SaveChanges();

            return RedirectToAction("Categories", "Admin");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category? category = _context.Categories
                .Include(x => x.Parameters)
                .FirstOrDefault(x => x.Id == id);

            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(
            int id,
            string categoryName,
            List<int> parameterIds,
            List<string> parameterNames,
            List<ParameterType> parameterTypes,
            List<bool> parameterRequired)
        {
            Category? category = _context.Categories
                .Include(x => x.Parameters)
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound();

            category.Name = categoryName;

            List<CategoryParameter> removedParameters = category.Parameters
                .Where(x => !parameterIds.Contains(x.Id))
                .ToList();

            _context.CategoryParameters.RemoveRange(removedParameters);

            for (int i = 0; i < parameterNames.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(parameterNames[i]))
                    continue;

                if (parameterIds[i] == 0)
                {
                    category.Parameters.Add(new CategoryParameter
                    {
                        Name = parameterNames[i],
                        Type = parameterTypes[i],
                        IsRequired = parameterRequired[i]
                    });
                }
                else
                {
                    CategoryParameter? existing = category.Parameters
                        .FirstOrDefault(x => x.Id == parameterIds[i]);

                    if (existing != null)
                    {
                        existing.Name = parameterNames[i];
                    }
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Categories", "Admin");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Category? category = _context.Categories
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);

            _context.SaveChanges();

            return RedirectToAction("Categories", "Admin");
        }
    }
}
