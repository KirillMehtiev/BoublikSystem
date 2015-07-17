using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BoublikSystem.Entities;
using BoublikSystem.Models;

namespace BoublikSystem.Controllers
{
    [Authorize(Roles = "admin, cook")]
    public class CookController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Cook
        public ActionResult Index()
        {
            return View();
        }

        // GET: /cook/WayBill
        public ActionResult CreateWayBill()
        {
            WayBill wayBill = new WayBill { SalesPointId = 364 };
            List<Product> products = context.Products.ToList();
            List<SelectListItem> adrressList = CreateAddresList(context.SalePoints.ToList());

            context.WayBills.Add(wayBill);

            // Добавляем бил в дб что-бы получить ид для била
            if (context.WayBills.Any(w => w.SalesPointId == 364))
                wayBill = context.WayBills.First(w => w.SalesPointId == 364);

            WayBillModel wayBillModel = new WayBillModel
            {
                CreatedWayBill = wayBill,
                Products = products,
                Address = adrressList
            };


            return View(wayBillModel);
        }

        public ViewResult AddProductToWayBill(int idProduct)
        {
            return new ViewResult();
        }

        // GET: /cook/ShowProducts
        public ActionResult ShowProducts()
        {
            return View();
        }

        

        private List<SelectListItem> CreateAddresList(List<SalePoint> salesList)
        {
            List<SelectListItem> answer = new List<SelectListItem>();

            if (salesList != null && salesList.Count > 0)
            {
                for (int i = 0; i < salesList.Count; i++)
                {
                    answer.Add(new SelectListItem
                    {
                        Text = salesList[i].Adress,
                        Value = salesList[i].Id.ToString()
                    });
                }
            }

            return answer;
        }
    }
}