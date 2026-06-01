using Microsoft.EntityFrameworkCore;
using TradeNest.Data;
using TradeNest.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add SQLite database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.EnsureCreated();

    if (!context.Users.Any())
    {
        User user = new()
        {
            Username = "test",
            Password = "test",
            Role = "Admin",
            IsActive = true
        };

        context.Users.Add(user);
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
