using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model
{
    public class SearchParameter
    {
        public List<string> SearchColumn { get; set; }
        public ReadOnlyCollection<string> SearchColumnValue { get; set; }
        public int StartIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecordCount { get; set; }
        public int SearchedCount { get; set; }
        public bool IsFilterValue { get; set; }
    }
    public class ParkSearchParameters
    {
        public Int32 Id { get; set; }
        public string ParkName { get; set; }
        public string SpacesToRent { get; set; }
        public string SpacesToOwn { get; set; }
        public string PhysicalAddress { get; set; }
        public string State { get; set; }
        public string PhysicalZip { get; set; }
        public bool? IsActive { get; set; }
    }
}
