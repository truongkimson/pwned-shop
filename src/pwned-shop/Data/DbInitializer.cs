using System;
using System.Linq;  
using pwned_shop.Models;

namespace pwned_shop.Data
{
    public static class DbInitializer
    {
        public static void Initialize(PwnedShopDb db)
        {
            db.Database.EnsureCreated();

            if (db.Users.Any())
            {
                return;
            }

            var u = new User
            {
                FirstName = "Alfreds",
                LastName = "Futterkiste",
                PasswordHash = "NNVgyjH2Fodc/EP8O9D65CX15E48/vyIIHuHKSiH6z8=",
                Salt = "P83Nl8w1ASxSfp1rR5Oi2Q==",
                DOB = new DateTime(1988, 7, 6),
                Email = "dprice@msn.com",
                Address = "Obere Str. 57"
            };

            db.Users.Add(u);
            db.SaveChanges();
        }
    }
}
