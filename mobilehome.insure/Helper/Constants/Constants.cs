using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mobilehome.insure.Helper.Constants
{
    public static class Constants
    {
        public static Dictionary<int, string> InstallmentList;
         static Constants()
        {
             InstallmentList = new Dictionary<int, string>();
                InstallmentList.Add(0, "");
                InstallmentList.Add(1, "Full Payment");
                InstallmentList.Add(2, "2 Payments");
                InstallmentList.Add(3, "3 Payments");
                InstallmentList.Add(4, "4 Payments");
        }
    }

    public enum ParkSiteStatus
    {
        InForce,
        Expired,
        Cancelled,
    }

    public enum ModeOfPayment
    {
        CreditCard,
        CheckOrMoneyOrder
    }
}