using System;
using System.Collections.Generic;

namespace MobileHome.Insure.Model.Appraisal
{
    public partial class AreaFactor
    {
        public int Id { get; set; }
        public Nullable<int> Area { get; set; }
        public Nullable<decimal> Factor { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> isActive { get; set; }
    }
}
