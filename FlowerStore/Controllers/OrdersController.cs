using FlowerStore.Data;
using FlowerStore.Extensions;
using FlowerStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowerStore.Controllers
{
    public class OrdersController : Controller
    {
        private const string CartKey = "Cart";            // same key used by CartController
        private const string UserEmailKey = "UserEmail"; // set by AccountController.Login (Joud)
        private const string UserNameKey = "UserName";   // full name, set by AccountController.Login

        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        private List<CartItem> GetCart()
        {
            return HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();
        }

        // Email of the logged-in user (null = not logged in)
        private string? CurrentUser => HttpContext.Session.GetString(UserEmailKey);

        // Sends the cart and its total to the Checkout page
        private void PrepareCheckoutPage(List<CartItem> cart)
        {
            ViewData["Cart"] = cart;
            ViewData["Total"] = cart.Sum(item => item.Total);
        }

        // ---------------------------------------------------------------
        // GET: /Orders/Checkout  -> shows the cart summary and the delivery form
        // ---------------------------------------------------------------
        public IActionResult Checkout()
        {
            // Only logged-in users can place an order (Session check)
            if (CurrentUser == null)
            {
                TempData["Message"] = "Please log in to complete your order.";
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path.ToString() });
            }

            var cart = GetCart();
            if (cart.Count == 0)
            {
                TempData["Message"] = "Your cart is empty. Add some flowers first! 🌸";
                return RedirectToAction("Index", "Cart");
            }

            PrepareCheckoutPage(cart);
            // Fill the receiver name with the user's name to save typing (they can change it)
            return View(new Order
            {
                CustomerName = HttpContext.Session.GetString(UserNameKey) ?? string.Empty,
                DeliveryDate = DateTime.Today.AddDays(1)
            });
        }

        // ---------------------------------------------------------------
        // POST: /Orders/Checkout  -> receives the form and saves the order (INSERT)
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(Order order)
        {
            if (CurrentUser == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path.ToString() });
            }

            var cart = GetCart();
            if (cart.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            // Extra server-side check that the attributes cannot do
            if (order.DeliveryDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(Order.DeliveryDate), "Delivery date cannot be in the past.");
            }

            if (!ModelState.IsValid)
            {
                PrepareCheckoutPage(cart);
                return View(order); // show the form again with the error messages
            }

            // Fill the values that come from the server, not from the form
            order.UserEmail = CurrentUser;
            order.OrderDate = DateTime.Now;
            order.Total = cart.Sum(item => item.Total);

            foreach (var item in cart)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity
                });
            }

            _context.Orders.Add(order);   // saves the order and its items together
            _context.SaveChanges();

            HttpContext.Session.Remove(CartKey); // empty the cart after ordering

            return RedirectToAction(nameof(Confirmation), new { id = order.Id });
        }

        // ---------------------------------------------------------------
        // GET: /Orders/Confirmation/5  -> "Thank you" page with the order details
        // ---------------------------------------------------------------
        public IActionResult Confirmation(int id)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);

            // Users can only see their own orders
            if (order == null || order.UserEmail != CurrentUser)
            {
                return NotFound();
            }

            return View(order);
        }

        // ---------------------------------------------------------------
        // GET: /Orders/MyOrders  -> list of the logged-in user's orders (RETRIEVE)
        // ---------------------------------------------------------------
        public IActionResult MyOrders()
        {
            if (CurrentUser == null)
            {
                TempData["Message"] = "Please log in to see your orders.";
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path.ToString() });
            }

            var orders = _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserEmail == CurrentUser)   // only this user's orders
                .OrderByDescending(o => o.OrderDate)      // newest first
                .ToList();

            return View(orders);
        }
    }
}
