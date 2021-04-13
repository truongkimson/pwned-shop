using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using pwned_shop.Utils;
using pwned_shop.Data;
using pwned_shop.Models;

namespace pwned_shop.Controllers
{
    public class OrderController : Controller
    {
        private readonly PwnedShopDb db;
        public OrderController(PwnedShopDb db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            int userId = 2;
            User user = db.Users.FirstOrDefault(u => u.Id == userId);
            
            foreach (Order o in user.Orders)
            {
                foreach (OrderDetail od in o.OrderDetails)
                {
                    Debug.WriteLine($"{o.Timestamp}, {od.ActivationCode}, {od.Product.ImgURL}, {od.Product.ProductName}");
                }
            }
          
            //// TODO: retrieve order history of current user
            return Content($"{user.Id}");
        }

        public IActionResult Detail(string orderId)
        {
            // TODO: retrieve particular order details
            return Content("Not implemented yet");
        }

        [Authorize]
        public IActionResult Checkout()
        {
            // TODO: convert current shopping cart to a successful order
            // Show activation codes
            ViewData["codes"] = ActivationCodeGenerator.GetCode();
            
            return View();
        }
    }
}
