using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pwned_shop.Models
{
    public class Product
    {
        public string ProductId { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public string RatingESRB { get; set; }
        public float UserRating { get; set; }
        public string ImgURL { get; set; }
        public string SteamLink { get; set; }
        public float Discount { get; set; }
    }
}
