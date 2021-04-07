using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace pwned_shop.Models
{
    public class OrderDetail
    {
        [Required]
        public string OrderId { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string ActivationCode { get; set; }
        public string ReceiverEmail { get; set; }

        public virtual Product Product { get; set; }
    }
}
