using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoublikSystem.Entities
{
    public class ProductToWayBill
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int WayBillId { get; set; }
       
        public double Count { get; set; }

        public virtual Product Product { get; set; }
        public virtual WayBill WayBill { get; set; }
    }
}