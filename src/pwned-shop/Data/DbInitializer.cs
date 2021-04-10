using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
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

            // populate Users table using data from csv/UserProfile.csv
            var rows = ReadCsv("Data/csv/UserProfile.csv");
            string dateFormat = "dd/mm/yyyy";
            for (int i = 1; i < rows.Count; i++)
            {
                var row = rows[i];
                User u = new User()
                {
                    Id = Convert.ToInt32(row[0]),
                    FirstName = row[1],
                    LastName = row[2],
                    PasswordHash = row[3],
                    Salt = row[4],
                    DOB = DateTime.ParseExact(row[5], dateFormat, null),
                    Email = row[6],
                    Address = row[7]
                };

                db.Users.Add(u);
            }

            // populate Ratings table using data from csv/RatingESRB.csv
            rows = ReadCsv("Data/csv/RatingESRB.csv");
            for (int i = 1; i < rows.Count; i++)
            {
                var row = rows[i];
                Rating r = new Rating()
                {
                    ESRBRating = row[0],
                    RatingDesc = row[1],
                    AgeGroup = row[2],
                };

                db.Ratings.Add(r);
            }
            

            // populate Products table using data from csv/Product.csv
            rows = ReadCsv("Data/csv/Product.csv");
            for (int i = 1; i < rows.Count; i++)
            {
                var row = rows[i];
                Product p = new Product()
                {
                    Id = Convert.ToInt32(row[0]),
                    ProductName = row[1],
                    ProductDesc = row[2],
                    CatTags = row[3],
                    UnitPrice = (float)Convert.ToDouble(row[4]),
                    ESRBRating = row[5],
                    ImgURL = row[6]
                };

                db.Products.Add(p);
            }
            db.SaveChanges();
        }

        public static List<string[]> ReadCsv(string path)
        {
            List<string[]> rows = new List<string[]>();
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var row = reader.ReadLine();
                    var values = CSVParser.Split(row);

                    // clean up the fields (remove " and leading spaces)
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].TrimStart(' ', '"');
                        values[i] = values[i].TrimEnd('"');
                    }

                    rows.Add(values);
                }
            }

            return rows;
        }
    }
}
