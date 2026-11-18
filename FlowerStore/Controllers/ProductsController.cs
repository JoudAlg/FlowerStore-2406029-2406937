using Microsoft.AspNetCore.Mvc;

namespace FlowerStore.Controllers
{
    // Student 1: products list, search, and admin CRUD go here
    public class ProductsController : Controller
    {
        // GET: /Products  (was products.html)
        public IActionResult Index()
        {
            return View();
        }
    }
}
