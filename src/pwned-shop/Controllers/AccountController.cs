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
            return Content($"Password hash is: {result[0]}\n" +
                $"Salt is: {result[1]}");
        }
    }
}
