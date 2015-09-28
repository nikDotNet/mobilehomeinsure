using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MobileHome.Insure.Model;

namespace mobilehome.insure.Models.Rental
{
    public class NotifyViewModel
    {

        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Zip Code")]
        public string Zip { get; set; }
    }
}