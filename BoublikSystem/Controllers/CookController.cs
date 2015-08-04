using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
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
        private static ApplicationDbContext context = new ApplicationDbContext();// БД
        private static List<Product> products; // список всей продукции
        private static List<SelectListItem> adrressList;
        private static Dictionary<Product, double> _billsList_view = new Dictionary<Product, double>(); // дабавленые продукты в накладную

        public CookController()
        {
            products = context.Products.ToList();
            adrressList = CreateAddresList(context.SalePoints.ToList());

        }

        // GET: Cook
        public ActionResult Index()
        {
            return View();
        }


        // GET: /cook/WayBill
        public ActionResult CreateWayBill()
        {
            

            return View(GetWayBillModel());
        }

        [HttpPost]
        public ActionResult CreateWayBill(WayBillModel wayBillModel)
        {
            if ((_billsList_view.Count > 0) && (wayBillModel.SelectedAdress != null))
            {

                // Что бы получить id для WayBill нужно его добавить в ДБ, затем считать
                int idSelectedAdress = Convert.ToInt32(wayBillModel.SelectedAdress);
                int futureId = 0;
                WayBill wayBill = new WayBill { SalesPointId = idSelectedAdress };
                context.WayBills.Add(wayBill);
                context.SaveChanges();
                
                foreach (var item in _billsList_view)
                {
                    ProductToWayBill productToWayBill = new ProductToWayBill
                    {
                        Count = item.Value,
                        ProductId = item.Key.Id,
                        WayBillId = wayBill.Id
                    };

                    context.ProductToWayBills.Add(productToWayBill);
                }

                context.SaveChanges();

                _billsList_view.Clear();

                
            }
            else
            {
                // todo: make some validation msg
            }


            return View(GetWayBillModel());

        }

        public ActionResult _AddProductToWayBill(int id, double count = -1)
        {
            string checkForPoint;

            checkForPoint = Request["countField"].ToString().Replace(".", ",");

            count = Convert.ToDouble(checkForPoint);

            var recivedProduct = context.Products.Find(id);

            if ((_billsList_view.Count > 0) && (_billsList_view.ContainsKey(recivedProduct)))
            {
                _billsList_view[recivedProduct] += count;
            }
            else
            {
                _billsList_view.Add(recivedProduct, count);
            }

            return PartialView(_billsList_view);

        }


        public ActionResult _MW_SelectCount(int id)
        {
            return PartialView(id);
            //TODO: waybillid должно обьявляться только после нажатия кнопки "отправить"
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

        private WayBillModel GetWayBillModel()
        {
            WayBillModel wayBillModel = new WayBillModel
            {
                Products = products,
                Address = adrressList
            };

            return wayBillModel;
        }
    }
}