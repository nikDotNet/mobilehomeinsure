using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Appraisal;
using MobileHome.Insure.Model.Rental;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobileHome.Insure.Web.Models.Admin
{
    public class ListNotifyViewModel
    {
        public List<ParkNotify> ListNotify = new List<ParkNotify>();

        public ParkNotify notifyObj { get; set; }

        public ListNotifyViewModel()
        {
          
        }
    }
}