using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BoublikSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BoublikSystem.Controllers
{
    public class AdminController : Controller
    {
        private static ApplicationDbContext dbContext = new ApplicationDbContext();
        private ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));
        private RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));

        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin
        public ActionResult CrudUser()
        {
            List<ApplicationUser> users = manager.Users.ToList();
            List<UserRoleModels> model = new List<UserRoleModels>();
            IList<string> usersRoles;
            string roles = null;

            for (int i = 0; i < users.Count; i++)
            {
                // Полечение всех ролей для юзера
                usersRoles = manager.GetRoles(users[i].Id);
                for (int j = 0; j < usersRoles.Count; j++)
                {
                    roles += usersRoles[j].ToString()+" ";
                }

                // Создание модели для отображения информации о юзере
                model.Add(new UserRoleModels {User = manager.FindById(users[i].Id), Role = roles});
                roles = null;
            }

            return View(model);
        }

        // GET: Admin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //todo : начни отсюда
            ApplicationUser applicationUser = manager.FindByName(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                //todo: ошибка если нажать создать и не заполнить поля

                if ((applicationUser.UserName == null) | (applicationUser.PasswordHash == null))
                    return View(applicationUser);

                var hasher = new PasswordHasher();

                applicationUser.PasswordHash = hasher.HashPassword(applicationUser.PasswordHash);

                manager.Create(applicationUser);

                return RedirectToAction("CrudUser");
            }

            return View(applicationUser);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = manager.FindByName(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = manager.FindByName(applicationUser.Id);

                var hasher = new PasswordHasher();

                user.PasswordHash = hasher.HashPassword(applicationUser.PasswordHash);

                manager.Update(user);

                return RedirectToAction("CrudUser");
            }
            return View(applicationUser);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = manager.FindByName(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = manager.FindByName(id);
            manager.Delete(applicationUser);
            return RedirectToAction("CrudUser");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                manager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
