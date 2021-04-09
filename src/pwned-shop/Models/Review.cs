using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace pwned_shop.Models
{
    public class Review
    {
        [Required]
        [MaxLength(5)]
        public string UserId { get; set; }
        [Required]
        [MaxLength(5)]
        public string ProductId { get; set; }
        [Required]
        public DateTime ReviewDate { get; set; }
        [Required]
        [MaxLength(200)]
        public string ReviewContent { get; set; }
        [Required]
        public int StarAssigned { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
