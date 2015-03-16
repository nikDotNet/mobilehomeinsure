using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model.Appraisal
{
    public class OptionsFactor
    {
        public int Id { get; set; }
        public int OptionsTypeId { get; set; }
        public int StateId { get; set; }
        public int ManufacturerId { get; set; }
        public decimal Rate { get; set; }
        public decimal Factor { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual OptionsType OptionsType { get; set; }
        public virtual State State { get; set; }
    }
}
