using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mobilehome.insure.Helper.Utility
{
    public static class Utils
    {
        public static List<string> GetListFromEnum(Type t)
        {
            return Enum.GetNames(t).ToList();
        }
    }
}