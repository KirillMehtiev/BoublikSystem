using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Conventions;
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
        private static ApplicationDbContext context = new ApplicationDbContext(); // БД
        private static List<Product> products; // список всей продукции

        
        private static List<SelectListItem> adrressList;

        private static Dictionary<Product, double> _billsList_view = new Dictionary<Product, double>();
        // дабавлены продукты в накладную

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
        [HttpGet]
        public ActionResult CreateWayBill()
        {

            ViewBag.IsError = false;
            ViewBag.Success = false;

            return View(GetWayBillModel());
        }


        [HttpPost]
        public ActionResult CreateWayBill(WayBillModel wayBillModel)
        {
            ViewBag.DefaultSelected = "Выберите адрес доставки";
            ViewBag.SelectedItem = wayBillModel.SelectedAdress;
            ViewBag.BillsCount = _billsList_view.Count;

            if ((_billsList_view.Count > 0) && (wayBillModel.SelectedAdress != null))
            {

                // todo: add to bd and clean list

                
                
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
                ViewBag.IsError = false;
                ViewBag.Success = true;

            }
            else
            {
                ViewBag.Success = false;
                ViewBag.IsError = true;
                // todo: make some validation msg
            }


            return View(GetWayBillModel());

        }

        public ActionResult _AddProductToWayBill(int id = -1, double count = -1)
        {
            string checkForPoint;

            try
            {
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
            }
            catch
            {
            }

            return PartialView(_billsList_view);

        }
        /// <summary>
        /// Удаление позиции в накладной
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteBillFromBillsList(int id)
        {
            var recivedProduct = context.Products.Find(id);
            _billsList_view.Remove(recivedProduct);
            _AddProductToWayBill();
            return PartialView("_AddProductToWayBill");
        }



        public ActionResult _MW_SelectCount(int id)
        {

            return PartialView(id);
            //TODO: waybillid должно обьявляться только после нажатия кнопки "отправить"
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
        #region ADD/Delete/EDIT PRODUCT

        // GET: /cook/ShowProducts
        public ActionResult ShowProducts()
        {
            return View(context.Products.ToList());
        }


        // GET: Product/Details/5
        public ActionResult ProductDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult CreateNewProduct()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewProduct([Bind(Include = "Id,Name,Price,MeasurePoint")] Product product)
        {
            if (ModelState.IsValid)
            {
                context.Products.Add(product);
                context.SaveChanges();
                return RedirectToAction("ShowProducts");
            }

            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct([Bind(Include = "Id,Name,Price,MeasurePoint")] Product prdct)
        {
            if (ModelState.IsValid)
            {
                //context.Entry(product).State = EntityState.Modified;
                //context.SaveChanges();
                //return RedirectToAction("ShowProducts");
                var productToUpdate = context.Products.Find(prdct.Id);
                try
                {
                    context.Entry(productToUpdate).CurrentValues.SetValues(prdct);
                    context.SaveChanges();

                    return RedirectToAction("ShowProducts");
                }
                catch (DataException  /*dex*/ )
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("",
                        "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }

            }
            return View(prdct);
        }

        // GET: Product/Delete/5
        public ActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProductConfirmed(int id)
        {
            Product product = context.Products.Find(id);
            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction("ShowProducts");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        context.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
        //TODO: РАЗОБРАТЬСЯ С БД

        #endregion
    }
}