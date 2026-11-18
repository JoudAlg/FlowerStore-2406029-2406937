using System.Diagnostics;
using FlowerStore.Data;
using FlowerStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowerStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Home/Index
        // Shows the first 3 products from the database as "Featured Products"
        public IActionResult Index()
        {
            var featured = _context.Products
                .OrderBy(p => p.Id)
                .Take(3)
                .ToList();

            return View(featured);
        }

        // GET: /Home/About
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
