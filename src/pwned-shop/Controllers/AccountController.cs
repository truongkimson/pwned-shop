using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Diagnostics;
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


        public IActionResult Login(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginDetails login, string returnUrl)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == login.Email);
            if (user != null)
            {
                string pwdHash = PasswordHasher.Hash(login.Password, user.Salt);
                if (pwdHash == user.PasswordHash)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("email", user.Email),
                        new Claim("role", "Member"),
                        new Claim("fullName", user.FirstName + user.LastName),
                        new Claim("userId", user.Id.ToString())
                    };

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5) // authentication ticket expiry
                    };

                    await HttpContext.SignInAsync(new ClaimsPrincipal(
                        new ClaimsIdentity(claims, "Cookies", "username", "role")),
                            authProperties);

                    return Redirect(returnUrl == null ? "/" : returnUrl);
                }
                else
                {
                    TempData["error"] = "Invalid password";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                TempData["error"] = "Invalid account";
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
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

        public IActionResult Denied()
        {
            return Content("Not implemented yet");
        }
    }
}
