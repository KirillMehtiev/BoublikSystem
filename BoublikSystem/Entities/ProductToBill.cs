using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoublikSystem.Entities
{
    public class ProductToBill
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BillId { get; set; }

        public Bill Bill { get; set; }
        public Product Product { get; set; }
    }
}