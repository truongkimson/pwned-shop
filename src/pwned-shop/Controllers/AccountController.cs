using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Login([FromForm] LoginDetails login)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == login.Email);
            if (user != null)
            {
                string pwdHash = PasswordHasher.Hash(login.Password, user.Salt);
                if (pwdHash == user.PasswordHash)
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    TempData["error"] = "Incorrect password";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                TempData["error"] = "Account not found";
                return RedirectToAction("Login");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Product");
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
