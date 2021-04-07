using System;
using System.Collections.Generic;
using pwned_shop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace pwned_shop.Data
{
    public class PwnedShopDb : DbContext
    {
        protected IConfiguration configuration;
        public PwnedShopDb(DbContextOptions<PwnedShopDb> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
