using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BoublikSystem.Entities
{
    public class Product
    {

        public int Id { get; set; }
         [Required(ErrorMessage = "Введите корректное имя!")]
        public string Name { get; set; }
       
        public decimal Price { get; set; }
        public string MeasurePoint { get; set; }
        
    }
}