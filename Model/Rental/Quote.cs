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
            this.ParkSites = new List<ParkSite>();
            this.Payments = new HashSet<Payment>();
        }

        
        public string ProposalNumber { get; set; }
        public Nullable<decimal> PersonalProperty { get; set; }
        public Nullable<decimal> Liability { get; set; }
        public Nullable<decimal> Deductible { get; set; }
        public Nullable<decimal> LOU { get; set; }
        public Nullable<decimal> MedPay { get; set; }
        public Nullable<decimal> Premium { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<int> NoOfInstallments { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }

        public bool SendLandLord { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<bool> IsParkSitePolicy { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<ParkSite> ParkSites { get; set; }
    }
}
