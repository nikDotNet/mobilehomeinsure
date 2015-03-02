using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Appraisal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobileHome.Insure.Web.Models.Admin
{
    public class ListManufacturerFactorViewModel
    {
        public List<ManufacturerFactor> ListManufacturerFactor = new List<ManufacturerFactor>();
        public ListManufacturerFactorViewModel()
        {

        }
    }
}