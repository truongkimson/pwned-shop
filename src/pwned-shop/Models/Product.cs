using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace pwned_shop.Models
{
    public class Product
    {
        [Required]
        [MaxLength(5)]
        public string ProductId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public double Price { get; set; }
        public string RatingESRB { get; set; }
        public float UserRating { get; set; }
        [Required]
        public string ImgURL { get; set; }
        public string SteamLink { get; set; }
        public float Discount { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
