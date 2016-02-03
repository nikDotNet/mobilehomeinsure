using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mobilehome.insure.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int NoOfNewCustomersInLast1Day { get; set; }
        public int NoOfNewCustomersInLast3Days { get; set; }
        public int NoOfNewCustomersInLast1Week { get; set; }

        public int NoOfNewPoliciesGeneratedInLast1Day { get; set; }
        public int NoOfNewPoliciesGeneratedInLast3Days { get; set; }
        public int NoOfNewPoliciesGeneratedInLast1Week { get; set; }

        public decimal PremiumChargedInLast1Day { get; set; }
        public decimal PremiumChargedInLast3Days { get; set; }
        public decimal PremiumChargedInLast1Week { get; set; }

        public bool IsPaymentProcessorLive { get; set; }
        public bool IsAegisServiceLive { get; set; }
    }
}