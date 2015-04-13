using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileHome.Insure.Model.Appraisal
{
    public partial class AreaFactor : Base.BaseEntity
    {
        [Required(ErrorMessage="Please enter area")]
        public Nullable<int> Area { get; set; }

        [Required(ErrorMessage="Please enter factor")]
        public Nullable<decimal> Factor { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> isActive { get; set; }
    }
}
