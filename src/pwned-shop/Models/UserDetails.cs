using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pwned_shop.Models
{
    public class UserDetails
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int ContactNum { get; set; }
        public string Country { get; set; }
    }
}
