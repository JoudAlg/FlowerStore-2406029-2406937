using FlowerStore.Data;
using Microsoft.EntityFrameworkCore;
using FlowerStore.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Database (Entity Framework Core + SQL Server LocalDB)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Sessions (used by the cart and by login)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();
// Create the database and a default Admin account the first time the site runs
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // applies all migrations automatically

    if (!db.Users.Any(u => u.Role == "Admin"))
    {
        var admin = new User
        {
            FullName = "Store Admin",
            Email = "admin@flowerstore.com",
            Role = "Admin"
        };
        admin.PasswordHash = new PasswordHasher<User>().HashPassword(admin, "Admin@1234");
        db.Users.Add(admin);
        db.SaveChanges();
    }
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();      // must come after UseRouting and before the routes
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
