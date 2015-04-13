using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileHome.Insure.Model.Appraisal
{
    public partial class ManufacturerFactor : Base.BaseEntity
    {
        [Display(Name = "Manufacturer")]
       [Required(ErrorMessage="Please enter manufacturer")]
        public Nullable<int> ManufacturerId { get; set; }

        [Required(ErrorMessage="Please enter factor")]
        public Nullable<decimal> Factor { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> isActive { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
    }
}
