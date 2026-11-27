
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FlowerStore.Data;
using FlowerStore.Models;

namespace FlowerStore.Controllers
{
    // Student 2: Registration, Login, Session, Cookies, Logout
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        // returnUrl = the page the user wanted before being asked to log in
        public IActionResult Login(string? returnUrl)
        {
            // Read remembered email from Cookie
            ViewBag.RememberedEmail =
                Request.Cookies["RememberedEmail"] ?? "";
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            string FullName,
            string Email,
            string Password,
            string ConfirmPassword)
        {
            FullName = FullName?.Trim() ?? "";
            Email = Email?.Trim().ToLowerInvariant() ?? "";

            if (string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError(
                    "", "All fields are required.");
                return View();
            }

            if (FullName.Length > 100 ||
                !new System.ComponentModel.DataAnnotations.EmailAddressAttribute()
                    .IsValid(Email))
            {
                ModelState.AddModelError(
                    "", "Please enter valid information.");
                return View();
            }

            if (Password.Length < 8 || Password != ConfirmPassword)
            {
                ModelState.AddModelError(
                    "", "Passwords must match and contain at least 8 characters.");
                return View();
            }

            if (await _context.Users.AnyAsync(u => u.Email == Email))
            {
                ModelState.AddModelError(
                    "", "This email is already registered.");
                return View();
            }

            var user = new User
            {
                FullName = FullName,
                Email = Email,
                Role = "Customer"
            };

            // Store a password hash, not the original password
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Account created successfully! Please log in. 🌸";
            return RedirectToAction("Login");
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string Email,
            string Password,
            bool RememberMe,
            string? returnUrl)
        {
            Email = Email?.Trim().ToLowerInvariant() ?? "";

            // Keep the typed email and the return page if the login fails
            ViewBag.RememberedEmail = Email;
            ViewBag.ReturnUrl = returnUrl;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == Email);

            if (user == null)
            {
                ModelState.AddModelError(
                    "", "Invalid email or password.");
                return View();
            }

            var hasher = new PasswordHasher<User>();

            var result = hasher.VerifyHashedPassword(
                user, user.PasswordHash, Password ?? "");

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(
                    "", "Invalid email or password.");
                return View();
            }

            // Save login information and role in Session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role);

            // Remember Me: store only the email in a Cookie
            if (RememberMe)
            {
                Response.Cookies.Append(
                    "RememberedEmail",
                    user.Email,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7),
                        HttpOnly = true,
                        Secure = Request.IsHttps,
                        SameSite = SameSiteMode.Lax,
                        IsEssential = true
                    });
            }
            else
            {
                Response.Cookies.Delete("RememberedEmail");
            }

            // Go back to the page the user wanted (e.g. Checkout), if it is inside our site
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["Message"] = "You have been logged out. See you soon! 👋";
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/AccessDenied
        // Shown when a logged-in Customer tries to open an Admin page
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
