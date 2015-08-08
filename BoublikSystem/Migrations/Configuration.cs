

using System.Collections.Generic;
using BoublikSystem.Entities;
using BoublikSystem.Models;
using WebGrease.Css.Extensions;

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

                // Create list of users to add in db
                var users = new List<ApplicationUser>
                {
                    new ApplicationUser {SallerLocation = 2, UserName = "admin", PasswordHash = hasher.HashPassword("admin"), PhoneNumber = "096-37-99-604"},
                    new ApplicationUser {SallerLocation = 2,UserName = "cook1", PasswordHash = hasher.HashPassword("cook1"), PhoneNumber = "096-37-99-604"},
                    new ApplicationUser {SallerLocation = 3, UserName = "seller", PasswordHash = hasher.HashPassword("seller"), PhoneNumber = "096-37-99-604"}
                };


                for (int i = 0; i < users.Count; i++)
                {
                    userManager.Create(users[i]);
                }

                // Create some roles
                roleManager.Create(new IdentityRole("admin"));
                roleManager.Create(new IdentityRole("cook"));
                roleManager.Create(new IdentityRole("seller"));

                // Add roles to users
                userManager.AddToRole(users[0].Id, "admin");
                userManager.AddToRole(users[1].Id, "cook");
                userManager.AddToRole(users[2].Id, "seller");

                // Create shops and their location = SalePoint
                List<SalePoint> salePoints = new List<SalePoint>
                {
                    new SalePoint {Adress = "ТРК Украина"},
                    new SalePoint {Adress = "Ак. Павлова"}
                };

                // Add salepaoints and save all changes to db
                context.SalePoints.Add(salePoints[0]);//1
                context.SalePoints.Add(salePoints[1]);//2
                context.SaveChanges();

                // Create a list of product
                List<Product> products = new List<Product>
                {
                    new Product{ Name = "Бублик",MeasurePoint = "шт",Price = 5.00m},
                    new Product{ Name = "Печенье",MeasurePoint = "кг",Price = 25.00m},
                };

                context.Products.AddRange(products);
                context.SaveChanges();

                // Create a wayBill for test purpose
                List<WayBill> wayBills = new List<WayBill>
                {
                    new WayBill { SalesPointId = 2}
                };

                context.WayBills.AddRange(wayBills);
                context.SaveChanges();

                List<ProductToWayBill> productToWay = new List<ProductToWayBill>
                {
                    new ProductToWayBill { WayBillId = 1, ProductId = 3,Count = 20},
                    new ProductToWayBill {WayBillId = 1,ProductId = 3,Count = 5}
                };

                context.ProductToWayBills.AddRange(productToWay);
                context.SaveChanges();



            }
        }
    }
}
