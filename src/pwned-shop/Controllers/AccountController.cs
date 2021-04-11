using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pwned_shop.Utils;
using pwned_shop.BindingModels;
using pwned_shop.Data;

namespace pwned_shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly PwnedShopDb db;

        public AccountController(PwnedShopDb db)
        {
            this.db = db;
        }


        public IActionResult Login()
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginDetails login, string salt)
        {
            var result = PasswordHasher.Hash(login.Password, salt);
            // TODO: verify against db if credentials provided are valid and redirect to "next" page
            return Content($"Password hash is: {result}");
        }

        public IActionResult Logout()
        {
            // TODO: Log out action, clear session, redirect to landing page
            return Content("Not yet implemented");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] UserRegDetails user)
        {
            var result = PasswordHasher.CreateHash(user.Password);
            // TODO: Register action, validate credentials data and persist in db
            // redirect to account create successful page
            return Content($"Password hash is: {result[0]}\n" +
                $"Salt is: {result[1]}");
        }
    }
}
