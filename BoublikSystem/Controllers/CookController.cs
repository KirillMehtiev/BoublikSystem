using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BoublikSystem.Controllers
{
    [Authorize(Roles = "admin, cook")]
    public class CookController : Controller
    {
        // GET: Cook
        public ActionResult Index()
        {
            return View();
        }
    }
}