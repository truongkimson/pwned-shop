using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pwned_shop.Utils;
using pwned_shop.Data;
using pwned_shop.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace pwned_shop.Controllers
{
    public class OrderController : Controller
    {
        private readonly PwnedShopDb db;
        public OrderController(PwnedShopDb db)
        {
            this.db = db;
        }
        [Authorize]
        public IActionResult Index()
        {
            ViewData["UserId"] = User;

            string userId1 = User.FindFirst("userId").Value;
            int userId = int.Parse(userId1);
            User user = db.Users.FirstOrDefault(u => u.Id == userId);
            List<OrderViewModel> ListOfOVM = new List<OrderViewModel>();
            
            //Extracting the data and including it into a list of objectviewmodel.
            foreach (Order o in user.Orders)
            {
                foreach (OrderDetail od in o.OrderDetails)
                {
                    //Debug.WriteLine($"{o.Timestamp}, {od.ActivationCode}, {od.Product.ImgURL}, {od.Product.ProductName}, {od.Product.ProductDesc}");
                    OrderViewModel temp = new OrderViewModel();
                    temp.ImgURL = od.Product.ImgURL;
                    temp.ProductName = od.Product.ProductName;
                    temp.ProductDesc = od.Product.ProductDesc;
                    temp.Timestamp = o.Timestamp;
                    temp.ActivationCode = od.ActivationCode;

                    ListOfOVM.Add(temp);
                }
            }

            //grouping the list by 
            List<OrderViewModel> ListOfOVM2 = new List<OrderViewModel>();
            var q = ListOfOVM.GroupBy(o => new 
            {
                o.ImgURL ,
                o.ProductName,
                o.ProductDesc,
                o.Timestamp
            });

            ViewData["OVMList"] = ListOfOVM;
            return View();
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

