namespace AppleShop.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity.EntityFramework;
    using AppleShop.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    internal sealed class Configuration : DbMigrationsConfiguration<AppleShop.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppleShop.Models.ApplicationDbContext context)
        {
            // --- BẮT ĐẦU CODE TẠO ROLE VÀ ADMIN USER ---
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Tạo role "Admin" nếu nó chưa tồn tại
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            // Tạo user "admin@youremail.com" nếu nó chưa tồn tại
            var user = userManager.FindByName("admin@appleshop.com");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "admin@appleshop.com",
                    Email = "admin@appleshop.com",
                };
                var result = userManager.Create(user, "Admin@123"); // <-- Mật khẩu admin

                if (result.Succeeded)
                {
                    // Thêm user vừa tạo vào role "Admin"
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
            // Thêm Category mẫu
            var categories = new List<Category>
    {
        // Cập nhật Category "iPhone"
        new Category {
            Name = "iPhone",
            Description = "Khám phá các dòng iPhone mới nhất với hiệu năng đỉnh cao và thiết kế sang trọng.",
            ImageUrl = "https://store.storeimages.cdn-apple.com/8756/as-images.apple.com/is/iphone-15-pro-finish-select-202309-6-7inch-naturaltitanium?wid=5120&hei=2880&fmt=p-jpg&qlt=80&.v=1692845699232"
        },
        // Cập nhật Category "MacBook"
        new Category {
            Name = "MacBook",
            Description = "MacBook siêu mạnh mẽ với chip Apple M series, hoàn hảo cho công việc và sáng tạo.",
            ImageUrl = "https://store.storeimages.cdn-apple.com/8756/as-images.apple.com/is/mbp14-spaceblack-select-202310?wid=904&hei=840&fmt=jpeg&qlt=90&.v=1697230830200"
        }
    };
            // Phương thức AddOrUpdate sẽ tự động cập nhật các bản ghi có sẵn
            categories.ForEach(c => context.Categories.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();

            // Thêm Product mẫu
            var products = new List<Product>
    {
        new Product { Name = "iPhone 15 Pro Max", Description = "Titan tự nhiên, Chip A17 Pro.", Price = 34990000, CategoryId = categories.Single(c => c.Name == "iPhone").Id, ImageUrl = "https://cdn.tgdd.vn/Products/Images/42/305658/iphone-15-pro-max-blue-thumbnew-600x600.jpg",IsFeatured = true },
        new Product { Name = "iPhone 14 Pro", Description = "Màu Tím sâu, Dynamic Island.", Price = 27990000, CategoryId = categories.Single(c => c.Name == "iPhone").Id, ImageUrl = "https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-tim-thumb-600x600.jpg" ,IsFeatured = false},
        new Product { Name = "MacBook Pro M3", Description = "Chip Apple M3, Màn hình Liquid Retina XDR.", Price = 49990000, CategoryId = categories.Single(c => c.Name == "MacBook").Id, ImageUrl = "https://cdn.tgdd.vn/Products/Images/44/318321/macbook-pro-16-inch-m3-pro-2023-18-core-cpu-18gb-512gb-bh-thumb-600x600.jpg",IsFeatured = true }
    };

            products.ForEach(p => context.Products.AddOrUpdate(s => s.Name, p));
            context.SaveChanges();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
