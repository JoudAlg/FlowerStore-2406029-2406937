using FlowerStore.Data;
using FlowerStore.Models;
using Microsoft.AspNetCore.Mvc;

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
        // RETRIEVE + SEARCH
        // GET: /Products  or  /Products?search=rose
        // ---------------------------------------------------------------
        public IActionResult Index(string? search)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                products = products.Where(p =>
                    p.Name.Contains(search) ||
                    (p.Description != null && p.Description.Contains(search)));
            }

            ViewData["Search"] = search;
            return View(products.OrderBy(p => p.Name).ToList());
        }

        // ---------------------------------------------------------------
        // Admin page: table of all products with Edit / Delete buttons
        // GET: /Products/Manage
        // ---------------------------------------------------------------
        public IActionResult Manage()
        {
            // Student 2: when login is ready, only logged-in users may open admin pages:
            // if (HttpContext.Session.GetString("UserName") == null)
            //     return RedirectToAction("Login", "Account");

            return View(_context.Products.OrderBy(p => p.Id).ToList());
        }

        // ---------------------------------------------------------------
        // INSERT
        // ---------------------------------------------------------------

        // GET: /Products/Create  -> shows the empty form
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Products/Create  -> receives the form data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            // Extra server-side check: no two products with the same name
            if (_context.Products.Any(p => p.Name == product.Name))
            {
                ModelState.AddModelError("Name", "A product with this name already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(product); // show the form again with the error messages
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            TempData["Message"] = $"\"{product.Name}\" was added successfully.";
            return RedirectToAction(nameof(Manage));
        }

        // ---------------------------------------------------------------
        // UPDATE
        // ---------------------------------------------------------------

        // GET: /Products/Edit/5  -> shows the form filled with the product data
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: /Products/Edit/5  -> saves the changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (_context.Products.Any(p => p.Name == product.Name && p.Id != product.Id))
            {
                ModelState.AddModelError("Name", "Another product already has this name.");
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            TempData["Message"] = $"\"{product.Name}\" was updated successfully.";
            return RedirectToAction(nameof(Manage));
        }

        // ---------------------------------------------------------------
        // DELETE
        // ---------------------------------------------------------------

        // GET: /Products/Delete/5  -> asks "Are you sure?"
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: /Products/Delete/5  -> deletes the product
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                TempData["Message"] = $"\"{product.Name}\" was deleted.";
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}
