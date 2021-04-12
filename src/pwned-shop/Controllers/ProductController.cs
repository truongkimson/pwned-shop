using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using pwned_shop.Data;

namespace pwned_shop.Controllers
{



    public class ProductController : Controller
    {
        private readonly PwnedShopDb db;
        public ProductController(PwnedShopDb db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            ViewData["penis"] = db.Products.ToList();
            return View();
        }

        public IActionResult Search(string query)
        {
            // TODO: retrieve list of products from db based on provided query
            // Clean up query to prevent SQL injection? is it necessary for EF Core?
            return Content("Not yet implemented");
        }

        public IActionResult Detail(string productId)
        {
            // TODO: retrieve product details based on productId given in params
            return Content("Not yet implemented");
        }
    }
}
