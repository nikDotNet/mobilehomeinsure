using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MobileHome.Insure.Model.Appraisal
{
    public class OptionsType:Base.BaseEntity
    {
        public OptionsType()
        {
            this.OptionsFactors = new List<OptionsFactor>();
        }

        [Display(Name = "Option Name")]
        public string Name { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public virtual ICollection<OptionsFactor> OptionsFactors { get; set; }
    }
}
