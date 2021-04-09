using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace pwned_shop.Models
{
    public class Rating
    {
        [Required]
        public string ESRBRating { get; set; }
        [Required]
        public string RatingDesc { get; set; }
        public string AgeGroup { get; set; }

        public virtual Product Product { get; set; }
    }
}
