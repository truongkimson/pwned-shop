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
            if (!User.Identity.IsAuthenticated)
            {
                var cartList = new List<Cart>(); 
                var cartDict = HttpContext.Session.GetJson<Dictionary<int, int>>("cart");

                if (cartDict == null || cartDict.Count == 0)
                    return View("EmptyCart");

                // seems like extra work, consider storing List<Cart> in Session State instead
                foreach (KeyValuePair<int,int> c in cartDict)
                {
                    cartList.Add(new Cart()
                    {
                        ProductId = c.Key,
                        Qty = c.Value,
                        Product = db.Products.FirstOrDefault(p => p.Id == c.Key)
                    });
                }

                ViewData["cartList"] = cartList;
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
            int productId = cu.ProductId; int qty = cu.Qty;
            if (qty < 0)
                return Json(new
                {
                    success = false
                });

            int cartCount = 0;
            float subTotal = 0;
            float total = 0;
            // if user is not logged in, update cart data in Session State as a Jsonified dict
            if (!User.Identity.IsAuthenticated)
            {
                var cartList = HttpContext.Session.GetJson<Dictionary<int, int>>("cart");

                // check if "cart" exists in Session data
                if (cartList != null)
                {
                    // update cart item qty if cart item exists, otherwise add new cart item
                    int result;
                    if (cartList.TryGetValue(productId, out result))
                        cartList[productId] = qty;
                    else
                        cartList.Add(productId, qty);
                }
                // create new cratList Dict if there isn't one in session
                else
                {
                    cartList = new Dictionary<int, int>();
                    cartList.Add(productId, qty);
                }

                // update "cart" Session data
                HttpContext.Session.SetJson("cart", cartList);

                // get latest "cartCount" and set to Session data
                cartCount = GetCartCount(cartList);
                HttpContext.Session.SetInt32("cartCount", cartCount);

                // for debugging, to delete
                foreach (KeyValuePair<int, int> c in cartList)
                {
                    Debug.WriteLine($"Prod: {c.Key} - {c.Value}");
                }
                Debug.WriteLine("Cart count: " + cartCount);

                subTotal = db.Products.FirstOrDefault(p => p.Id == productId).UnitPrice * qty;
                total = GetTotal(cartList);
                return Json(new
                {
                    success = true,
                    cartCount = cartCount,
                    subTotal = subTotal,
                    total = total
                });
            }

            // else user is logged in, update cart data in SQL db Cart table
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

            return Json(new
            {
                success = true,
                cartCount = cartCount,
                subTotal = subTotal,
                total = total
            });
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            Debug.WriteLine("Prod Id: " + productId);
            int cartCount;
            // if user is not logged in, update cart data in Session State as a Jsonified dict
            if (!User.Identity.IsAuthenticated)
            {
                var cartList = HttpContext.Session.GetJson<Dictionary<int, int>>("cart");
                // check if "cart" exists in Session data
                if (cartList != null)
                {
                    // check if cart item for this product exists
                    int result;
                    if (cartList.TryGetValue(productId, out result))
                        cartList[productId] = result + 1;
                    else
                        cartList.Add(productId, 1);
                }
                // create new cratList Dict if there isn't one in session
                else
                {
                    cartList = new Dictionary<int, int>()
                    {
                        { productId, 1 }
                    };
                }

                // update "cart" Session data
                HttpContext.Session.SetJson("cart", cartList);

                // get latest "cartCount" and set to Session data
                cartCount = GetCartCount(cartList);
                HttpContext.Session.SetInt32("cartCount", cartCount);

                // for debugging, to delete
                foreach (KeyValuePair<int, int> c in cartList)
                {
                    Debug.WriteLine($"Prod: {c.Key} - {c.Value}");
                }
                Debug.WriteLine("Cart count: " + cartCount);

                return Json(new{ success = true, cartCount = cartCount }) ;
            }

            // else user is logged in, update cart data in SQL db Cart table
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

            // get latest "cartCount" and add to Session data
            cartCount = db.Users.FirstOrDefault(u => u.Id == userId).Carts.Sum(c => c.Qty);
            HttpContext.Session.SetInt32("cartCount", cartCount);

            // for debugging, to delete
            foreach (var c in db.Users.FirstOrDefault(u => u.Id == userId).Carts)
            {
                Debug.WriteLine($"Prod: {c.ProductId} - {c.Qty}");
            }
            Debug.WriteLine("Cart count: " + cartCount);

            return Json(new { success = true, cartCount = cartCount });
        }

        public IActionResult RemoveFromCart(int productId)
        {
            Debug.WriteLine(productId);
            int cartCount;
            if (!User.Identity.IsAuthenticated)
            {
                var cartList = HttpContext.Session.GetJson<Dictionary<int, int>>("cart");

                // for debugging, to delete
                Debug.WriteLine(cartList.Remove(productId));

                // update "cart" Session data
                HttpContext.Session.SetJson("cart", cartList);

                // get latest "cartCount" and set to Session data
                cartCount = GetCartCount(cartList);
                HttpContext.Session.SetInt32("cartCount", cartCount);
            }
            else
            {
                int userId = Convert.ToInt32(User.FindFirst("userId").Value);
                var cart = db.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);
                db.Carts.Remove(cart);

                db.SaveChanges();

                // get latest "cartCount" and add to Session data
                cartCount = db.Users.FirstOrDefault(u => u.Id == userId).Carts.Sum(c => c.Qty);
                HttpContext.Session.SetInt32("cartCount", cartCount);
            }
            return RedirectToAction("Index");
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

        protected float GetTotal(Dictionary<int,int> cartList)
        {
            float total = 0;
            foreach (KeyValuePair<int,int> cart in cartList)
            {
                float unitPrice = db.Products.FirstOrDefault(p => p.Id == cart.Key).UnitPrice;
                total += cart.Value * unitPrice;
            }
            return total;
        }
    }
}
