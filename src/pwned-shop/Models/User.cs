using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;


namespace pwned_shop.Models
{
    public class User
    {
        [Required]
        [MaxLength(5)]
        public int UserId { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string PasswordHash { get; set; }
        [Required]
        [MaxLength(30)]
        public string Salt { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        [MaxLength(25)]
        public string Email { get; set; }
        [Required]
        [MaxLength(60)]
        public string Address { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
