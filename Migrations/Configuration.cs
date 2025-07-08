namespace AppleShop.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity.EntityFramework;
    using AppleShop.Models;
    internal sealed class Configuration : DbMigrationsConfiguration<AppleShop.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppleShop.Models.ApplicationDbContext context)
        {
            // Thêm Category mẫu
            var categories = new List<Category>
    {
        new Category { Name = "iPhone" },
        new Category { Name = "MacBook" }
    };
            categories.ForEach(c => context.Categories.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();

            // Thêm Product mẫu
            var products = new List<Product>
    {
        new Product { Name = "iPhone 15 Pro Max", Description = "Titan tự nhiên, Chip A17 Pro.", Price = 34990000, CategoryId = categories.Single(c => c.Name == "iPhone").Id, ImageUrl = "https://cdn.tgdd.vn/Products/Images/42/305658/iphone-15-pro-max-blue-thumbnew-600x600.jpg" },
        new Product { Name = "iPhone 14 Pro", Description = "Màu Tím sâu, Dynamic Island.", Price = 27990000, CategoryId = categories.Single(c => c.Name == "iPhone").Id, ImageUrl = "https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-tim-thumb-600x600.jpg" },
        new Product { Name = "MacBook Pro M3", Description = "Chip Apple M3, Màn hình Liquid Retina XDR.", Price = 49990000, CategoryId = categories.Single(c => c.Name == "MacBook").Id, ImageUrl = "https://cdn.tgdd.vn/Products/Images/44/318321/macbook-pro-16-inch-m3-pro-2023-18-core-cpu-18gb-512gb-bh-thumb-600x600.jpg" }
    };

            products.ForEach(p => context.Products.AddOrUpdate(s => s.Name, p));
            context.SaveChanges();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
