using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pwned_shop.BindingModels
{
    public class UserRegDetails
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int ContactNum { get; set; }
        public string Country { get; set; }
    }
}
