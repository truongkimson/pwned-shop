using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using pwned_shop.Models;
using System.Diagnostics;

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

            // populate Users table
            var rows = ReadCsv("Data/csv/UserProfile.csv");
            Debug.WriteLine(rows.Count);
            for (int i = 1; i < rows.Count; i++)
            {
                string format = "dd/mm/yyyy";
                var row = rows[i];
                User u = new User()
                {
                    Id = Convert.ToInt32(row[0]),
                    FirstName = row[1],
                    LastName = row[2],
                    PasswordHash = row[3],
                    Salt = row[4],
                    DOB = DateTime.ParseExact(row[5], format, null),
                    Email = row[6],
                    Address = row[7]
                };

                db.Users.Add(u);
                db.SaveChanges();
            }
        }

        public static List<string[]> ReadCsv(string path)
        {
            List<string[]> rows = new List<string[]>();
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var row = reader.ReadLine();
                    var values = row.Split(',');

                    rows.Add(values);
                }
            }

            return rows;
        }
    }
}
