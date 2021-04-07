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
        public string OrderId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
        public bool CheckoutStatus { get; set; }
        [MaxLength(8)]
        public string PromoCode { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
