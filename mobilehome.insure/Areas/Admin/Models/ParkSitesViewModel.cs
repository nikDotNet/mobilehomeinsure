using MobileHome.Insure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mobilehome.insure.Areas.Admin.Models
{
    public class ParkSitesViewModel
    {
        public ParkSitesViewModel()
            {

            }

            public List<ParkSite> ParkSites { get; set; }

            public ParkSite CurrentParkSite { get; set; }

            public List<State> States { get; set; }
        
    }
}