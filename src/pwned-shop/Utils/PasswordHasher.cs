using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace pwned_shop.Utils
{
    public class PasswordHasher
    {
        public static string Hash(string password)
        {
            // generate a 128-bit salt using a secure PRNG (pseudo-random number generator)
            byte[] salt = new byte[128 / 8]; // 128 bits / 8 bits per byte = 16 byte-long array
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt); //fills an array of bytes with a cryptographically strong random sequence of values
            }

            // derive 256-bit long hash using SHA256, 10,000 iterations
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
