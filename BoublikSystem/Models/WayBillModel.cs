using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoublikSystem.Entities;

namespace BoublikSystem.Models
{
    public class WayBillModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<SelectListItem> Address { get; set; }
        [Required (ErrorMessage = "Не выбрана точка доставки!")]

        public string SelectedAdress { get; set; }

    }
}