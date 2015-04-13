using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Appraisal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobileHome.Insure.Web.Models.Admin
{
    public class ListManufacturerViewModel
    {
        public List<Manufacturer> ListManufacturer = new List<Manufacturer>();

       public Manufacturer manufacturerObj { get; set; }

        public ListManufacturerViewModel()
        {

        }
    }
}