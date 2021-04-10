using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pwned_shop.Models
{
    public class Rating
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ESRBRating { get; set; }
        [Required]
        public string RatingDesc { get; set; }
        public string AgeGroup { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
