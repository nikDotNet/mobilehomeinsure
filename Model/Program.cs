using MobileHome.Insure.Model.Rental;
using System;
using System.Collections.Generic;

namespace MobileHome.Insure.Model
{
    public partial class Program : Base.BaseEntity
    {
        public Program()
        {
            this.InstallmentStructures = new List<InstallmentStructure>();
        }

        public string Name { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<decimal> ProcessingFeeInPercentage { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public virtual ICollection<InstallmentStructure> InstallmentStructures { get; set; }
        public virtual Company Company { get; set; }
    }
}

