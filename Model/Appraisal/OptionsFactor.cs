using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MobileHome.Insure.Model.Appraisal
{
    public class OptionsFactor:Base.BaseEntity
    {
        [Display(Name="Options Type")]
        [Required(ErrorMessage="Please select an Option Type")]
        public int OptionsTypeId { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage="Please select a state")]
        public int StateId { get; set; }

        [Display(Name="Manufacturer")]
        [Required(ErrorMessage="Please select a manufacturer")]
        public int ManufacturerId { get; set; }

        [Required(ErrorMessage="Please enter a rate")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage="Please enter factor")]
        public decimal Factor { get; set; }

        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual OptionsType OptionsType { get; set; }
        public virtual State State { get; set; }
    }
}
