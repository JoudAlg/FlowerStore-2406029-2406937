
using FlowerStore.Data;
using FlowerStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FlowerStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // ---------------------------------------------------------------
        // AUTHORIZATION: Only Admin can manage products
        // Products Index and Search remain public
        // ---------------------------------------------------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var action = RouteData.Values["action"]?.ToString();

            if (!string.Equals(
                action,
                nameof(Index),
                StringComparison.OrdinalIgnoreCase))
            {
                // Visitors must log in first
                if (!HttpContext.Session.GetInt32("UserId").HasValue)
                {
                    context.Result = RedirectToAction("Login", "Account");
                    return;
                }

                // Only Admin can access management actions
                var userRole = HttpContext.Session.GetString("UserRole");

                if (!string.Equals(
                    userRole?.Trim(),
                    "Admin",
                    StringComparison.OrdinalIgnoreCase))
                {
                    context.Result = new StatusCodeResult(403);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }

        // ---------------------------------------------------------------
        // RETRIEVE + SEARCH + FILTER BY CATEGORY (Public)
        // GET: /Products  or  /Products?search=rose  or  /Products?category=Roses
        // ---------------------------------------------------------------
        public IActionResult Index(string? search, string? category)
        {
            var products = _context.Products.AsQueryable();

            // Filter by category (only if it is one of our categories)
            if (!string.IsNullOrWhiteSpace(category) &&
                Product.Categories.Contains(category))
            {
                products = products.Where(p => p.Category == category);
            }
            else
            {
                category = null; // "All"
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                products = products.Where(p =>
                    p.Name.Contains(search) ||
                    (p.Description != null &&
                     p.Description.Contains(search)));
            }

            ViewData["Search"] = search;
            ViewData["Category"] = category;

            return View(
                products.OrderBy(p => p.Name).ToList());
        }

        // ---------------------------------------------------------------
        // MANAGE (Admin only)
        // ---------------------------------------------------------------
        public IActionResult Manage()
        {
            return View(
                _context.Products.OrderBy(p => p.Id).ToList());
        }

        // ---------------------------------------------------------------
        // INSERT (Admin only)
        // ---------------------------------------------------------------

        // GET: /Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (_context.Products.Any(p => p.Name == product.Name))
            {
                ModelState.AddModelError(
                    "Name",
                    "A product with this name already exists.");
            }

            if (!Product.Categories.Contains(product.Category))
            {
                ModelState.AddModelError(
                    "Category",
                    "Please choose a valid category.");
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            TempData["Message"] =
                $"\"{product.Name}\" was added successfully.";

            return RedirectToAction(nameof(Manage));
        }

        // ---------------------------------------------------------------
        // UPDATE (Admin only)
        // ---------------------------------------------------------------

        // GET: /Products/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: /Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (_context.Products.Any(p =>
                p.Name == product.Name && p.Id != product.Id))
            {
                ModelState.AddModelError(
                    "Name",
                    "Another product already has this name.");
            }

            if (!Product.Categories.Contains(product.Category))
            {
                ModelState.AddModelError(
                    "Category",
                    "Please choose a valid category.");
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            TempData["Message"] =
                $"\"{product.Name}\" was updated successfully.";

            return RedirectToAction(nameof(Manage));
        }

        // ---------------------------------------------------------------
        // DELETE (Admin only)
        // ---------------------------------------------------------------

        // GET: /Products/Delete/5
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: /Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();

                TempData["Message"] =
                    $"\"{product.Name}\" was deleted.";
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}
