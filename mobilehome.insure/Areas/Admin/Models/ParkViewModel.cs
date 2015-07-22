using MobileHome.Insure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mobilehome.insure.Areas.Admin.Models
{
    public class ParkViewModel
    {
        public ParkViewModel()
        {

        }

        public List<Park> Parks { get; set; }

        public Park CurrentPark { get; set; }

        public List<State> States { get; set; }
    }
}