using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using pwned_shop.Utils;
using pwned_shop.BindingModels;
using pwned_shop.Data;
using pwned_shop.Models;

namespace pwned_shop.Controllers
{
    public class CartController : Controller
    {
        private readonly PwnedShopDb db;

        public CartController(PwnedShopDb db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            // TODO: Retrieve cart from session data or wherever cart data is stored
            return View();
        }

        [HttpPost]
        public IActionResult UpdateCart([FromBody] CartUpdate cu)
        {
            int productId = cu.ProductId; int qty = cu.Qty;
            int cartCount;
            // if user is not logged in, update cart data in Session State as a Jsonified dict
            if (!User.Identity.IsAuthenticated)
            {
                var cartList = HttpContext.Session.GetJson<Dictionary<int, int>>("cart");
                if (cartList != null)
                {
                    int result;
                    if (cartList.TryGetValue(productId, out result))
                        cartList[productId] = (result + qty) < 0 ? 0 : (result + qty); // Make sure cart qty doesn't go below 0
                    else
                        // Only add new item to cart list when qty > 0
                        if (qty > 0)
                            cartList.Add(productId, qty);
                }
                else
                {
                    cartList = new Dictionary<int, int>();
                    // Only add new item to cart list when qty > 0
                    if (qty > 0)
                        cartList.Add(productId, qty);
                }

                HttpContext.Session.SetJson("cart", cartList);
                cartCount = GetCartCount(cartList);
                HttpContext.Session.SetInt32("cartCount", cartCount);
                // to delete
                foreach (KeyValuePair<int, int> c in cartList)
                {
                    Debug.WriteLine($"Prod: {c.Key} - {c.Value}");
                }
                Debug.WriteLine("Cart count: " + cartCount);
                return Json(new { success = true, cartCount = cartCount });
            }

            // else user is logged in, update cart data in SQL db Cart table
            int userId = Convert.ToInt32(User.FindFirst("userId").Value);
            var cart = db.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

            if (cart != null)
            {
                cart.Qty = (cart.Qty + qty) < 0 ? 0 : (cart.Qty + qty);
            }
            else
            {
                if (qty > 0)
                {
                    cart = new Cart() { UserId = userId, ProductId = productId, Qty = qty };
                    db.Carts.Add(cart);
                }
            }
            db.SaveChanges();

            cartCount = db.Users.FirstOrDefault(u => u.Id == userId).Carts.Sum(c => c.Qty);
            HttpContext.Session.SetInt32("cartCount", cartCount);
            // to delete
            Debug.WriteLine("Cart count: " + cartCount);

            return Json(new { success = true, cartCount = cartCount });
        }

        public IActionResult AddToCart(string productId)
        {
            // might not be necessary
            // TODO: Update cart's data in session data or wherever cart data is stored
            return Content($"Not implemented yet {HttpContext.Session.Id}");
        }

        public IActionResult RemoveFromCart(string productId)
        {
            HttpContext.Session.SetString("key", "value");
            // TODO: Update cart's data in session data or wherever cart data is stored
            string result = "";

            foreach (var k in HttpContext.Session.Keys)
            {
                result += k + " ";
            }
            return Content($"Not implemented yet {result}");
        }

        protected int GetCartCount(Dictionary<int,int> cartList)
        {
            int count = 0;
            foreach (KeyValuePair<int, int> cart in cartList)
            {
                count += cart.Value;
            }
            return count;
        }
    }
}
