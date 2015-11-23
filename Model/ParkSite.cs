using MobileHome.Insure.Model.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model
{
    public partial class ParkSite : Base.BaseEntity
    {
        public Nullable<int> ParkId { get; set; }
        public Nullable<int> SiteNumber { get; set; }
        public string TenantFirstName { get; set; }
        public string TenantLastName { get; set; }
        public string PhysicalAddress1 { get; set; }
        public string PhysicalAddress2 { get; set; }
        public string PhysicalCity { get; set; }
        public Nullable<int> PhysicalStateId { get; set; }
        public Nullable<int> PhysicalZip { get; set; }
        public Nullable<int> QuoteId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public virtual Park Park { get; set; }
        public virtual State State { get; set; }
        public virtual Quote Quote { get; set; }
    }
}
