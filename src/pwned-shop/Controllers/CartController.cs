using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using pwned_shop.Utils;
using pwned_shop.BindingModels;
using pwned_shop.Data;
using pwned_shop.Models;
using pwned_shop.ViewModels;

namespace pwned_shop.Controllers
{
    public class CartController : Controller
    {
        private readonly PwnedShopDb db;
        private readonly ILogger<CartController> _logger;

        public CartController(PwnedShopDb db, ILogger<CartController> logger)
        {
            this.db = db;
            _logger = logger;
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var cartList = HttpContext.Session.GetJson<CartListViewModel>("cart");

                if (cartList == null || cartList.List.Count == 0)
                    return View("EmptyCart");

                foreach (Cart c in cartList.List)
                {
                    c.Product = db.Products.FirstOrDefault(p => p.Id == c.ProductId);
                }

                ViewData["cartList"] = cartList.List;
            }
            else
            {
                var userId = Convert.ToInt32(User.FindFirst("userId").Value);
                var user = db.Users.FirstOrDefault(u => u.Id == userId);

                List<Cart> cartList = user.Carts.ToList();

                if (cartList.Count == 0)
                    return View("EmptyCart");

                ViewData["cartList"] = cartList;
            }

            return View();
        }

        [HttpPost]
        public IActionResult UpdateCart([FromBody] CartUpdate cu)
        {
            int productId; int qty;
            try
            {
                productId = cu.ProductId; qty = cu.Qty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Json( new
                {
                    success = false
                });
            }

            if (qty <= 0)
                return Json(new
                {
                    success = false
                });

            int cartCount;
            float subTotal;
            float total = 0;

            // if user is not logged in, update cart data in Session State as a Jsonified dict
            if (!User.Identity.IsAuthenticated)
            {
                var cartList = HttpContext.Session.GetJson<CartListViewModel>("cart");

                // check if "cart" exists in Session data
                if (cartList != null)
                {
                    // update cart item qty if cart item exists, otherwise add new cart item
                    cartList.UpdateCart(new Cart { ProductId = productId, Qty = qty });
                }
                // create new cratList Dict if there isn't one in session
                else
                {
                    cartList = new CartListViewModel();
                    cartList.UpdateCart(new Cart { ProductId = productId, Qty = qty });
                }

                // update "cart" Session data
                HttpContext.Session.SetJson("cart", cartList);

                // get latest "cartCount" and set to Session data
                cartCount = cartList.CartCount;
                HttpContext.Session.SetInt32("cartCount", cartCount);

                // for debugging, to delete
                foreach (Cart c in cartList.List)
                {
                    Debug.WriteLine($"Prod: {c.ProductId} - {c.Qty}");
                }
                Debug.WriteLine("Cart count: " + cartCount);

                subTotal = db.Products.FirstOrDefault(p => p.Id == productId).UnitPrice * qty;

                foreach(Cart c in cartList.List)
                {
                    var unitPrice = db.Products.FirstOrDefault(p => p.Id == c.ProductId).UnitPrice;
                    total += unitPrice * c.Qty;
                }
            }
            // else user is logged in, update cart data in SQL db Cart table
            else
            {
                int userId = Convert.ToInt32(User.FindFirst("userId").Value);
                var cart = db.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

                // update cart item's qty if exists, otherwise add new Cart object
                if (cart != null)
                {
                    cart.Qty = qty;
                }
                else
                {
                    cart = new Cart() { UserId = userId, ProductId = productId, Qty = qty };
                    db.Carts.Add(cart);
                }
                db.SaveChanges();

                // get latest "cartCount" and set to Session data
                cartCount = db.Users.FirstOrDefault(u => u.Id == userId).Carts.Sum(c => c.Qty);
                HttpContext.Session.SetInt32("cartCount", cartCount);

                // for debugging, to delete
                foreach (Cart c in db.Users.FirstOrDefault(u => u.Id == userId).Carts)
                {
                    Debug.WriteLine($"Prod: {c.ProductId} - {c.Qty}");
                }
                Debug.WriteLine("Cart count: " + cartCount);

                subTotal = cart.Product.UnitPrice * qty;

                foreach (Cart c in db.Users.FirstOrDefault(u => u.Id == userId).Carts)
                {
                    total += c.Product.UnitPrice * c.Qty;
                }
            }

            HttpContext.Session.SetInt32("cartCount", cartCount);
            return Json(new
            {
                success = true,
                cartCount = cartCount,
                subTotal = subTotal.ToString("C"),
                total = total.ToString("C")
            });
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            int cartCount;
            
            // if user is not logged in, update cart data in Session State as
            // a Jsonified CartList object
            if (!User.Identity.IsAuthenticated)
            {
                var cartList = HttpContext.Session.GetJson<CartListViewModel>("cart");

                // check if "cart" exists in Session data
                if (cartList != null)
                {
                    cartList.AddToCart(new Cart { ProductId = productId, Qty = 1 });
                }
                // create new new CartList object if there isn't one in session
                else
                {
                    cartList = new CartListViewModel();
                    cartList.AddToCart(new Cart { ProductId = productId, Qty = 1 });
                }

                // update "cart" Session data
                HttpContext.Session.SetJson("cart", cartList);
                // get latest "cartCount"
                cartCount = cartList.CartCount;

                // for debugging, to delete
                foreach (Cart c in cartList.List)
                {
                    Debug.WriteLine($"Prod: {c.ProductId} - {c.Qty}");
                }
                Debug.WriteLine("Cart count: " + cartCount);
            }
            // else user is logged in, update cart data in SQL db Cart table
            else
            {
                int userId = Convert.ToInt32(User.FindFirst("userId").Value);
                var cart = db.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

                // check if cart item for this product exists
                if (cart != null)
                {
                    cart.Qty += 1;
                }
                // create new Cart object if cart item doesnt exist
                else
                {
                    cart = new Cart() { UserId = userId, ProductId = productId, Qty = 1 };
                    db.Carts.Add(cart);
                }
                db.SaveChanges();

                // get latest "cartCount"
                cartCount = db.Users.FirstOrDefault(u => u.Id == userId).Carts.Sum(c => c.Qty);
                
                // for debugging, to delete
                foreach (var c in db.Users.FirstOrDefault(u => u.Id == userId).Carts)
                {
                    Debug.WriteLine($"Prod: {c.ProductId} - {c.Qty}");
                }
                Debug.WriteLine("Cart count: " + cartCount);
            }

            HttpContext.Session.SetInt32("cartCount", cartCount);
            return Json(new { success = true, cartCount = cartCount });
        }

        public IActionResult RemoveFromCart(int productId)
        {
            int cartCount;
            if (!User.Identity.IsAuthenticated)
            {
                var cartList = HttpContext.Session.GetJson<CartListViewModel>("cart");

                // for debugging, to delete
                int remove = cartList.RemoveFromCart(new Cart { ProductId = productId });
                Debug.WriteLine(remove);
                _logger.LogInformation(remove.ToString());

                // update "cart" Session data
                HttpContext.Session.SetJson("cart", cartList);

                // get latest "cartCount" and set to Session data
                cartCount = cartList.CartCount;
            }
            else
            {
                int userId = Convert.ToInt32(User.FindFirst("userId").Value);
                var cart = db.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);
                db.Carts.Remove(cart);

                db.SaveChanges();

                // get latest "cartCount" and add to Session data
                cartCount = db.Users.FirstOrDefault(u => u.Id == userId).Carts.Sum(c => c.Qty);
            }

            HttpContext.Session.SetInt32("cartCount", cartCount);
            return RedirectToAction("Index");
        }

 
    }
}
