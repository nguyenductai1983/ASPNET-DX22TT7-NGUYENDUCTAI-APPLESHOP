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
    //using Microsoft.AspNet.Identity.EntityFramework;
    internal sealed class Configuration : DbMigrationsConfiguration<AppleShop.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AppleShop.Models.ApplicationDbContext context)
        {
            // --- BẮT ĐẦU CODE TẠO ROLE VÀ ADMIN USER (PHIÊN BẢN CẢI TIẾN) ---
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string roleName = "Admin";
            string adminEmail = "admin@appleshop.com";
            string adminPassword = "Admin@123";

            // 1. Tạo role "Admin" nếu nó chưa tồn tại
            if (!roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole(roleName);
                roleManager.Create(role);
            }

            // 2. Tìm user admin
            var user = userManager.FindByEmail(adminEmail);

            // 3. Nếu user chưa tồn tại, tạo mới
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                };
                var result = userManager.Create(user, adminPassword);

                // Nếu tạo user thất bại, ném ra lỗi để dễ dàng debug
                if (!result.Succeeded)
                {
                    throw new System.Data.Entity.Validation.DbEntityValidationException(string.Join(";", result.Errors));
                }
            }

            // 4. Luôn kiểm tra và thêm user vào role "Admin" nếu chưa có
            if (!userManager.IsInRole(user.Id, roleName))
            {
                userManager.AddToRole(user.Id, roleName);
            }
            // --- KẾT THÚC CODE TẠO ROLE VÀ ADMIN USER ---
            // Thêm Category mẫu
            var categories = new List<Category>
    {
        // Cập nhật Category "iPhone"
        new Category {
            Name = "iPhone",
            Description = "Khám phá các dòng iPhone mới nhất với hiệu năng đỉnh cao và thiết kế sang trọng.",
            ImageUrl = "/Content/Images/Categories/iphone 14.jpg"
        },
        // Cập nhật Category "MacBook"
        new Category {
            Name = "MacBook",
            Description = "MacBook siêu mạnh mẽ với chip Apple M series, hoàn hảo cho công việc và sáng tạo.",
            ImageUrl = "/Content/Images/Categories/macbook.jpg"
        }
    };
            // Phương thức AddOrUpdate sẽ tự động cập nhật các bản ghi có sẵn
            categories.ForEach(c => context.Categories.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();

            // Thêm Product mẫu
            var products = new List<Product>
    {
        new Product { Name = "iPhone 15 Pro Max", Description = "Titan tự nhiên, Chip A17 Pro.", Price = 34990000, CategoryId = categories.Single(c => c.Name == "iPhone").Id, ImageUrl = "/Content/Images/Products/iphone.jpg",IsFeatured = true },
        new Product { Name = "iPhone 14 Pro", Description = "Màu Tím sâu, Dynamic Island.", Price = 27990000, CategoryId = categories.Single(c => c.Name == "iPhone").Id, ImageUrl = "/Content/Images/Products/iphone 14.jpg" ,IsFeatured = false},
        new Product { Name = "MacBook Pro M3", Description = "Chip Apple M3, Màn hình Liquid Retina XDR.", Price = 49990000, CategoryId = categories.Single(c => c.Name == "MacBook").Id, ImageUrl = "/Content/Images/Products/macbook.jpg",IsFeatured = true }
    };

            products.ForEach(p => context.Products.AddOrUpdate(s => s.Name, p));
            context.SaveChanges();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
