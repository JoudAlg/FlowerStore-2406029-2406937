using Microsoft.AspNetCore.Mvc;

namespace FlowerStore.Controllers
{
    // Student 2: register, login (session + "remember me" cookie) and logout go here
    public class AccountController : Controller
    {
        // GET: /Account/Login  (was login.html)
        public IActionResult Login()
        {
            return View();
        }

        // GET: /Account/Register  (was register.html)
        public IActionResult Register()
        {
            return View();
        }
    }
}
