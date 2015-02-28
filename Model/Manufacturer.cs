using System;
using System.Collections.Generic;
using MobileHome.Insure.Model.Appraisal;

namespace MobileHome.Insure.Model
{
    public partial class Manufacturer 
    {
        public Manufacturer()
        {
            this.ManufacturerFactors = new List<ManufacturerFactor>();
        }

        public int Id { get; set; }
        public string Manufacturer1 { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> isActive { get; set; }
        public virtual ICollection<ManufacturerFactor> ManufacturerFactors { get; set; }

       
    }
}
