using Microsoft.AspNetCore.Mvc;

namespace FlowerStore.Controllers
{
    // Student 1: the session-based cart goes here
    public class CartController : Controller
    {
        // GET: /Cart  (was cart.html)
        public IActionResult Index()
        {
            return View();
        }
    }
}
