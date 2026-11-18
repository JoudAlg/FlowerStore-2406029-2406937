using FlowerStore.Data;
using FlowerStore.Extensions;
using FlowerStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowerStore.Controllers
{
    // The shopping cart is stored in the SESSION (on the server),
    // so it stays while the user moves between pages.
    public class CartController : Controller
    {
        private const string CartKey = "Cart";
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Reads the cart from the session (or returns an empty cart)
        private List<CartItem> GetCart()
        {
            return HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();
        }

        // Saves the cart back into the session
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetObject(CartKey, cart);
        }

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = GetCart();
            ViewData["Total"] = cart.Sum(item => item.Total);
            return View(cart);
        }

        // POST: /Cart/Add  (from the "Add to Cart" buttons)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int productId, string? returnUrl)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCart();
            var existing = cart.FirstOrDefault(item => item.ProductId == productId);

            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }

            SaveCart(cart);
            TempData["Message"] = $"{product.Name} added to cart! 🌸";

            // Go back to the page the user came from
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index", "Products");
        }

        // POST: /Cart/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int productId)
        {
            var cart = GetCart();
            cart.RemoveAll(item => item.ProductId == productId);
            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/Clear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove(CartKey);
            TempData["Message"] = "Shopping cart cleared successfully!";

            return RedirectToAction(nameof(Index));
        }
    }
}
