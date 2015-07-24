

using System.Collections.Generic;
using BoublikSystem.Entities;
using BoublikSystem.Models;

namespace BoublikSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BoublikSystem.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "BoublikSystem.Models.ApplicationDbContext";
        }

        protected override void Seed(BoublikSystem.Models.ApplicationDbContext context)
        {
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var hasher = new PasswordHasher();
                // Create users
                var users = new List<ApplicationUser>
                {
                    new ApplicationUser {UserName = "admin", PasswordHash = hasher.HashPassword("admin"), PhoneNumber = "096-37-99-604"},
                    new ApplicationUser {UserName = "cook1", PasswordHash = hasher.HashPassword("cook1"), PhoneNumber = "096-37-99-604"},
                    new ApplicationUser {UserName = "seller", PasswordHash = hasher.HashPassword("seller"), PhoneNumber = "096-37-99-604"}
                };


                for (int i = 0; i < users.Count; i++)
                {
                    userManager.Create(users[i]);
                }

                // Create roles
                roleManager.Create(new IdentityRole("admin"));
                roleManager.Create(new IdentityRole("cook"));
                roleManager.Create(new IdentityRole("seller"));

                // Set roles to users
                userManager.AddToRole(users[0].Id, "admin");
                userManager.AddToRole(users[1].Id, "cook");
                userManager.AddToRole(users[2].Id, "seller");

                // Create sales point
                List<SalePoint> salePoints = new List<SalePoint>
                {
                    new SalePoint {Adress = "��� �������"},
                    new SalePoint {Adress = "��. �������"}
                };

                context.SalePoints.Add(salePoints[0]);//1
                context.SalePoints.Add(salePoints[1]);//2
                context.SaveChanges();

                // Create a list of products
                List<Product> products = new List<Product>
                {
                    new Product{ Name = "������",MeasurePoint = "��",Price = 5.00m},
                    new Product{ Name = "�������",MeasurePoint = "��",Price = 25.00m},
                };

                context.Products.AddRange(products);
                context.SaveChanges();

                // Create a waybills
                List<WayBill> wayBills = new List<WayBill>
                {
                    new WayBill { SalesPointId = 1}
                };

                context.WayBills.AddRange(wayBills);
                context.SaveChanges();
                
                // Create and test table in db
                List<ProductToWayBill> productToWay = new List<ProductToWayBill>
                {
                    new ProductToWayBill { WayBillId = 1, ProductId = 1,Count = 20},
                    new ProductToWayBill {WayBillId = 1,ProductId = 2,Count = 5}
                };

                context.ProductToWayBills.AddRange(productToWay);
                context.SaveChanges();

            }
        }
    }
}
