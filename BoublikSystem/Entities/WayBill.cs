using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoublikSystem.Entities
{
    public class WayBill
    {
        public int Id { get; set; }
        [Display(Name = "Адрес для отправки накладной")]
        public int SalesPointId { get; set; }

        public virtual ICollection<ProductToWayBill> ProductToWayBills { get; set; }
    }
}