﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pwned_shop.Models
{
    public class Product
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
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
        public string ESRBRating { get; set; }
        [Required]
        public string ImgURL { get; set; }
        public string SteamLink { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Rating Rating { get; set; }
    }
}
