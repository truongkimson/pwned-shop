using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pwned_shop.Utils;
using pwned_shop.BindingModels;

namespace pwned_shop.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginDetails login)
        {
            return Content($"Password hash is: {PasswordHasher.Hash(login.Password)}");
        }
    }
}
