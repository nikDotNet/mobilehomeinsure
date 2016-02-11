using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model.DTO
{
   public class ParkSiteDto
    {
      public int Id { get; set; }
      public string SiteNumber { get; set; }
      public long ParkId { get; set; }
       public string ParkName { get; set; }
       public string TenantFirstName { get; set; }
       public string TenantLastName { get; set; }
       public decimal Premium {get; set;}
       public decimal? PersonalProperty { get; set; }
       public decimal? Liability { get; set; }
       public string EffectiveDate { get; set; }
       public string ExpiryDate { get; set; }
       public string SiteRental { get; set; }
       public string CompanyName { get; set; }
    } 
}
