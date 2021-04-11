using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pwned_shop.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Retrieve cart from session data or wherever cart data is stored
            return View();
        }

        public IActionResult UpdateCart(string productId, int qty)
        {
            // TODO: Update cart's data in session data or wherever cart data is stored
            return Content("Not implemented yet");
        }

        public IActionResult AddToCart(string productId)
        {
            // TODO: Update cart's data in session data or wherever cart data is stored
            return Content("Not implemented yet");
        }

        public IActionResult RemoveFromCart(string productId)
        {
            // TODO: Update cart's data in session data or wherever cart data is stored
            return Content("Not implemented yet");
        }
    }
}
