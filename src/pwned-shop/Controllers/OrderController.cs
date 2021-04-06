using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pwned_shop.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            // TODO: retrieve order history of current user
            return Content("Not implemented yet");
        }

        public IActionResult Detail(string orderId)
        {
            // TODO: retrieve particular order details
            return Content("Not implemented yet");
        }
    }
}
