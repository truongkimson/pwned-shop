using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace pwned_shop.Models
{
    public class Discount
    {
        [Required]
        [MaxLength(8)]
        public string PromoCode { get; set; }
        public float DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [MaxLength(20)]
        public string Remarks { get; set; }

        public virtual Order Order { get; set; }
    }
}
