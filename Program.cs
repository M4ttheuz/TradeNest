using Microsoft.EntityFrameworkCore;
using TradeNest.Data;
using TradeNest.Models;
using TradeNest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add SQLite database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<UserService>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.EnsureCreated();

    var user = context.Users.FirstOrDefault(u => u.Login == "Admin");

    if (user != null)
    {
        user.Role = UserRole.Admin;
        context.SaveChanges();
    }


    if (!context.Categories.Any(c => c.Name == "Motoryzacja"))
    {
        Category carCategory = new()
        {
            Name = "Motoryzacja",
            Parameters = new List<CategoryParameter>
            {
                new CategoryParameter { Name = "Marka", Type = ParameterType.Text, IsRequired = true },
                new CategoryParameter { Name = "Model", Type = ParameterType.Text, IsRequired = true },
                new CategoryParameter { Name = "Rok produkcji", Type = ParameterType.Number, IsRequired = true },
                new CategoryParameter { Name = "Przebieg (km)", Type = ParameterType.Number, IsRequired = true },
                new CategoryParameter { Name = "Pojemność silnika (cm³)", Type = ParameterType.Number, IsRequired = false },
                new CategoryParameter { Name = "Uszkodzony", Type = ParameterType.Boolean, IsRequired = false }
            }
        };

        context.Categories.Add(carCategory);
        context.SaveChanges();
    }

    if (!context.Categories.Any(c => c.Name == "Elektronika"))
    {
        Category electronicsCategory = new()
        {
            Name = "Elektronika",
            Parameters = new List<CategoryParameter>
            {
                new CategoryParameter { Name = "Marka", Type = ParameterType.Text, IsRequired = true },
                new CategoryParameter { Name = "Model", Type = ParameterType.Text, IsRequired = true },
                new CategoryParameter { Name = "Stan", Type = ParameterType.Text, IsRequired = true },
                new CategoryParameter { Name = "Rok produkcji", Type = ParameterType.Number, IsRequired = false },
                new CategoryParameter { Name = "Oryginalne opakowanie", Type = ParameterType.Boolean, IsRequired = false }
            }
        };

        context.Categories.Add(electronicsCategory);
        context.SaveChanges();
    }

    if (!context.Categories.Any(c => c.Name == "Moda"))
    {
        Category fashionCategory = new()
        {
            Name = "Moda",
            Parameters = new List<CategoryParameter>
            {
                new CategoryParameter { Name = "Marka", Type = ParameterType.Text, IsRequired = false },
                new CategoryParameter { Name = "Rozmiar", Type = ParameterType.Text, IsRequired = true },
                new CategoryParameter { Name = "Kolor", Type = ParameterType.Text, IsRequired = false },
                new CategoryParameter { Name = "Materiał", Type = ParameterType.Text, IsRequired = false },
                new CategoryParameter { Name = "Stan", Type = ParameterType.Text, IsRequired = true },
            }
        };

        context.Categories.Add(fashionCategory);
        context.SaveChanges();
    }

    if (!context.Categories.Any(c => c.Name == "Nieruchomości"))
    {
        Category realEstateCategory = new()
        {
            Name = "Nieruchomości",
            Parameters = new List<CategoryParameter>
            {
                new CategoryParameter { Name = "Typ nieruchomości", Type = ParameterType.Text, IsRequired = true },
                new CategoryParameter { Name = "Miasto", Type = ParameterType.Text, IsRequired = true },
                new CategoryParameter { Name = "Powierzchnia (m²)", Type = ParameterType.Number, IsRequired = true },
                new CategoryParameter { Name = "Liczba pokoi", Type = ParameterType.Number, IsRequired = false },
                new CategoryParameter { Name = "Piętro", Type = ParameterType.Number, IsRequired = false },
                new CategoryParameter { Name = "Rok budowy", Type = ParameterType.Number, IsRequired = false },
                new CategoryParameter { Name = "Garaż", Type = ParameterType.Boolean, IsRequired = false }
            }
        };

        context.Categories.Add(realEstateCategory);
        context.SaveChanges();
    }

    if (!context.Categories.Any(c => c.Name == "Usługi"))
    {
        Category servicesCategory = new()
        {
            Name = "Usługi",
            Parameters = new List<CategoryParameter>
            {
                new CategoryParameter { Name = "Obszar działania", Type = ParameterType.Text, IsRequired = false },
                new CategoryParameter { Name = "Doświadczenie (lata)", Type = ParameterType.Number, IsRequired = false },
                new CategoryParameter { Name = "Czas realizacji (dni)", Type = ParameterType.Number, IsRequired = false },
                new CategoryParameter { Name = "Dojazd do klienta", Type = ParameterType.Boolean, IsRequired = false },
                new CategoryParameter { Name = "Faktura VAT", Type = ParameterType.Boolean, IsRequired = false },
                new CategoryParameter { Name = "Usługa online", Type = ParameterType.Boolean, IsRequired = false }
            }
        };

        context.Categories.Add(servicesCategory);
        context.SaveChanges();
    }

    if (!context.Categories.Any(c => c.Name == "Hobby"))
    {
        Category hobbyCategory = new()
        {
            Name = "Hobby",
            Parameters = new List<CategoryParameter>
            {
                new CategoryParameter { Name = "Marka", Type = ParameterType.Text, IsRequired = false },
                new CategoryParameter { Name = "Model", Type = ParameterType.Text, IsRequired = false },
                new CategoryParameter { Name = "Rok produkcji", Type = ParameterType.Number, IsRequired = false },
                new CategoryParameter { Name = "Kolekcjonerski", Type = ParameterType.Boolean, IsRequired = false },
                new CategoryParameter { Name = "Kompletne wyposażenie", Type = ParameterType.Boolean, IsRequired = false },
                new CategoryParameter { Name = "Oryginalne opakowanie", Type = ParameterType.Boolean, IsRequired = false }
            }
        };

        context.Categories.Add(hobbyCategory);
        context.SaveChanges();
    }
}

app.Run();
