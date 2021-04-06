using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pwned_shop.Utils;
using pwned_shop.BindingModels;

namespace pwned_shop.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginDetails login)
        {
            var result = PasswordHasher.CreateHash(login.Password);
            // TODO: verify against db if credentials provided are valid and redirect to "next" page
            return Content($"Password hash is: {result[0]}\n" +
                $"Salt is: {result[1]}");
        }

        public IActionResult Logout()
        {
            // TODO: Log out action, clear session, redirect to landing page
            return Content("Not yet implemented");
        }

        public IActionResult Register()
        {
            // TODO: Register action, validate credentials data and persist in db
            // redirect to account create successful page
            return Content("Not yet implemented");
        }
    }
}
