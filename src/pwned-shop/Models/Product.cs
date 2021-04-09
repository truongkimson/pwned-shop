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
        public int ProductId { get; set; }
        [Required]
        [MaxLength(30)]
        public string ProductName { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProductDesc { get; set; }
        [Required]
        [MaxLength(15)]
        public string ProductCat { get; set; }
        [Required]
        public float UnitPrice { get; set; }
        [Required]
        public string RatingESRB { get; set; }
        [Required]
        public string ImgURL { get; set; }
        public string SteamLink { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Rating Rating { get; set; }
    }
}
