using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace pwned_shop.Models
{
    public class Order
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
        [MaxLength(8)]
        public string PromoCode { get; set; }
        public bool CheckoutStatus { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Discount Discount { get; set; }
        public virtual User User { get; set; }

    }
}
