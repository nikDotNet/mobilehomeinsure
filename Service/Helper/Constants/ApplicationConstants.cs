using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Service.Helper.Constants
{
   public static class ApplicationConstants
    {
       public static string UnsubscribeText = "If you do not want to receive future communication from MobileHome.Insure. Please click here to Unsubscribe";

       public static List<string> installmentLevel1States = new List<string>(new string[] {"CO","MI","MS","TX"});

       public static List<string> installmentLevel2States = new List<string>(new string[] { "SC", "TN", "VA" });

       public static List<string> installmentLevel3States = new List<string>(new string[] { "NC" });
    }
}
