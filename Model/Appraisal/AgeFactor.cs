using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MobileHome.Insure.Model.Appraisal
{
    public partial class AgeFactor : Base.BaseEntity
    {
        [Display(Name="Age")]
        [Required(ErrorMessage="Please enter age")]
        public Nullable<int> Age { get; set; }
       
        [Display(Name="Factor")]
        [Required(ErrorMessage="Please enter factor")]
        public Nullable<decimal> Factor { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> isActive { get; set; }
    }
}
