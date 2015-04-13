using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Appraisal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobileHome.Insure.Web.Models.Admin
{
    public class ListStateFactorViewModel
    {
        public List<StateFactor> ListStateFactor = new List<StateFactor>();

      public  List<State> lstStates { get; set; }

        public StateFactor stateFactorObj { get; set; }

        public ListStateFactorViewModel()
        {

        }
    }
}