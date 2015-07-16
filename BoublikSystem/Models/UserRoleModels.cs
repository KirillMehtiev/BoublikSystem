using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace BoublikSystem.Models
{
    public class UserRoleModels
    {
        public ApplicationUser User { get; set; }
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}