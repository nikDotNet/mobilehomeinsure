using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileHome.Insure.Model.Appraisal
{
    public partial class StateFactor : Base.BaseEntity
    {
        [Display(Name="State")]
        [Required(ErrorMessage = "Please select a state")]
        public Nullable<int> StateId { get; set; }

        [Display(Name="Factor")]
        [Required(ErrorMessage="Please enter a factor")]
        public Nullable<decimal> Factor { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> isActive { get; set; }
        public virtual State State { get; set; }
    }
}
