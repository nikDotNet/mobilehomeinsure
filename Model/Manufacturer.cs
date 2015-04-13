using System;
using System.Collections.Generic;
using MobileHome.Insure.Model.Appraisal;
using System.ComponentModel.DataAnnotations;

namespace MobileHome.Insure.Model
{
    public partial class Manufacturer : Base.BaseEntity
    {
        public Manufacturer()
        {
            this.ManufacturerFactors = new List<ManufacturerFactor>();
            this.OptionsFactors = new List<OptionsFactor>();
        }

        [Display(Name = "Name")]
        [Required(ErrorMessage="Please enter a name")]
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> isActive { get; set; }
        public virtual ICollection<ManufacturerFactor> ManufacturerFactors { get; set; }
        public virtual ICollection<OptionsFactor> OptionsFactors { get; set; }

       
    }
}
