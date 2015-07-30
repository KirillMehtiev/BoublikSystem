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
        [HttpGet]
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

       
        private static List<Product> selectedItems = new List<Product>();
        //private static Dictionary<Product, Product_Data> selectedCountProducts = new Dictionary<Product, Product_Data>();
        private static List<ProductToWayBill> productToWayBill = new List<ProductToWayBill>();
        private static Dictionary<Product, ProductToWayBill> _billsList_view = new Dictionary<Product, ProductToWayBill>();
        static List<ProductToWayBill> prod = new List<ProductToWayBill>();
        //--
        private static Dictionary<int, ProductToWayBill> _billsList_viewByID = new Dictionary<int, ProductToWayBill>();
        public ActionResult _AddProductToWayBill(int id, int count = -1)
        {
            count = Convert.ToInt32(Request["countField"]);
             data = context.Products.ToList();
            try
            {
               foreach (var item in _billsList_view)
                {
                    if (item.Key.Id==id)
                    {
                        _billsList_view[item.Key].Count = count;
                    }
                  
                }
                
               // _billsList_view[data[0]].Count = count;
            
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return PartialView(_billsList_view);

        }

        private static List<Product> data = new List<Product>();
        public ActionResult _MW_SelectCount(int id, int wayBillId)//ProductToWayBill product_ToWay_Bill )
        {

         
             data = context.Products.ToList();
          
            _billsList_view.Add(data[id - 1], new ProductToWayBill() { ProductId = id});

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
    }
}