using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pwned_shop.Utils;

namespace pwned_shop.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            // TODO: retrieve order history of current user
            return View();
        }

        public IActionResult Detail(string orderId)
        {
            // TODO: retrieve particular order details
            return Content("Not implemented yet");
        }

        public IActionResult Checkout()
        {
            // TODO: convert current shopping cart to a successful order
            // Show activation codes
            ViewData["codes"] = ActivationCodeGenerator.GetCode();
            
            return View();
        }
    }
}
