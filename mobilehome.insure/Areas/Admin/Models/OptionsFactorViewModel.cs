using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MobileHome.Insure.Model.Appraisal;
using MobileHome.Insure.Model;

namespace mobilehome.insure.Areas.Admin.Models
{
    public class OptionsFactorViewModel
    {
        public OptionsFactor optionFactorObject { get; set; }


        public List<State> lstStates { get; set; }

        public List<Manufacturer> lstManufacturers { get; set; }

        public Dictionary<int,string> lstOptionsType { get; set; }

    }
}