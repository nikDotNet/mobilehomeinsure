using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model
{
    public class ZipInfo:Base.BaseEntity
    {
        public string Zip { get; set; }
        public string ZipKey { get; set; }
        public string City { get; set; }
        public string CityPreferred { get; set; }
        public string County { get; set; }
        public string StateAbbr { get; set; }
        public string CountyNumber { get; set; }
        public int TerritoryNumber { get; set; }
    }
}
