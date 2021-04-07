using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pwned_shop.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ActivationCode { get; set; }
        public string ReceiverEmail { get; set; }
    }
}
