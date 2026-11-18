using System.Diagnostics;
using FlowerStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowerStore.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/Index  (was index.html)
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/About  (was about.html)
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
