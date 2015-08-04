using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoublikSystem.Entities
{
    public class Bill
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime DataTime { get; set; }
        public int SalePointId { get; set; }

    }
}