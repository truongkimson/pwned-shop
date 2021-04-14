using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using pwned_shop.Utils;
using pwned_shop.Data;
using pwned_shop.Models;
using System.Data.Entity;

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

            //Add some cart data
            Cart mockCart1 = new Cart();
            mockCart1.UserId = userId;
            mockCart1.ProductId = 2;
            mockCart1.Qty = 3;

            Cart mockCart2 = new Cart();
            mockCart2.UserId = userId;
            mockCart2.ProductId = 4;
            mockCart2.Qty = 2;
           
            Debug.WriteLine("Test for mock data" + mockCart1.ProductId);

            db.Carts.Add(mockCart1);
            db.Carts.Add(mockCart2);
            db.SaveChanges();
            
            foreach (var test1 in db.Carts)
            {
                Debug.WriteLine(test1.ProductId);
            }

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
            //ViewData["codes"] = ActivationCodeGenerator.GetCode();
            List<Order> newOrderList = new List<Order>();
            List<OrderDetail> newOrderDetailsList = new List<OrderDetail>();
            int i = 0;

            //generate orderId
            //Add order and orderdetal data into database after purchase.
            var LatestOrderId = db.Orders.Max(o => o.Id);
            var newOrderId = LatestOrderId + 1;
            //test
            Debug.WriteLine("this is the checkout");
            List<Cart> userCart = new List<Cart>();
            string userId1 = User.FindFirst("userId").Value;
            int userId = int.Parse(userId1);
            userCart = db.Users.FirstOrDefault(u => u.Id == userId).Carts.ToList();

            //While we are adding order and orderdetail data into the database, we will populate the view data as well for the reciept
            
            List<CheckOutViewModel> recieptList = new List<CheckOutViewModel>();

            Order newOrder = new Order();
            newOrder.UserId = userId;
            newOrder.Id = newOrderId;
            newOrder.Timestamp = DateTime.Now;
            db.Orders.Add(newOrder);
            db.SaveChanges();
            Debug.WriteLine(newOrder.Id);
            Debug.WriteLine(newOrder);
            foreach (var cartItem in userCart)
            {
                while(i<cartItem.Qty)
                {
                    //Populate OrderDetail & add to database
                    OrderDetail newOrderDetail = new OrderDetail();
                    string activation = Guid.NewGuid().ToString();
                    newOrderDetail.ActivationCode = activation;
                    newOrderDetail.OrderId = newOrderId;
                    newOrderDetail.ProductId = cartItem.ProductId;
                    i++;
                    db.OrderDetails.Add(newOrderDetail);
                    db.SaveChanges();
                    Debug.WriteLine(newOrderDetail);
                    Debug.WriteLine(newOrderDetail.ActivationCode);

                    //populate the checkoutviewmodel
                    CheckOutViewModel reciept = new CheckOutViewModel();
                    reciept.ImgURL = cartItem.Product.ImgURL;
                    reciept.ProductName = cartItem.Product.ProductName;
                    reciept.ProductDesc = cartItem.Product.ProductDesc;
                    reciept.ActivationCode = activation;
                    reciept.Qty = cartItem.Qty;
                    reciept.UnitPrice = cartItem.Product.UnitPrice;

                    recieptList.Add(reciept);
                }
                i = 0;
            }

            //mapping the orderviewmodel into to the view using viewdata
            ViewData["RecieptView"] = recieptList;

            //Clearing the Cart table in database after purchase
           foreach (var cartDelete in userCart)
            {
                db.Carts.Remove(cartDelete);
            }
            db.SaveChanges();

            




            return View();
        }
    } 
}

