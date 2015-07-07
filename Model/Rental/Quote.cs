using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model.Rental
{
    public class Quote : Base.BaseEntity
    {
        public Quote() 
        {
            this.Payments = new HashSet<Payment>();
        }

        
        public string ProposalNumber { get; set; }
        public Nullable<decimal> PersonalProperty { get; set; }
        public Nullable<decimal> Liability { get; set; }
        public Nullable<decimal> Deductible { get; set; }
        public Nullable<decimal> Premium { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<int> NoOfInstallments { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
