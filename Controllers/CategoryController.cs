using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult Edit()
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

            return RedirectToAction("Index", "Home");
        }
    }
}
