using System;
using System.Collections.Generic;

namespace Model.Appraisal
{
    public partial class ManufacturerFactor
    {
        public int Id { get; set; }
        public Nullable<int> ManufacturerId { get; set; }
        public Nullable<decimal> Factor { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> isActive { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
    }
}
