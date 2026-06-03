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

    if (!context.Categories.Any(c => c.Name == "Samochody Osobowe"))
    {
        Category carCategory = new()
        {
            Name = "Samochody Osobowe",
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
}

app.Run();
