using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult UpdateCart(int productId, int qty)
        {
            Dictionary<int, int> outputDict = new Dictionary<int, int>();
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
                // for debugging
                outputDict = cartList;
                return Content("200 Success. " + String.Join(Environment.NewLine, outputDict));
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
            return Content($"200 Success. {cart.UserId}, {cart.ProductId}, {cart.Qty}");
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
    }
}
