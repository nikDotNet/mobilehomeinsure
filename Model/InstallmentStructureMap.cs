using System;
using System.Collections.Generic;

namespace MobileHome.Insure.Model
{
    public partial class InstallmentStructure : Base.BaseEntity
    {
        public string Name { get; set; }
        public Nullable<int> NoOfInstallments { get; set; }
        public Nullable<decimal> ProcessingFee { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public virtual Program Program { get; set; }
        public virtual State State { get; set; }
    }
}
