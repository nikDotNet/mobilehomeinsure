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
      public int SiteNumber { get; set; }
       public string ParkName { get; set; }
       public string TenantFirstName { get; set; }
       public string TenantLastName { get; set; }
       public string PhysicalCity { get; set; }
       public string PhysicalState {get; set;}
       public int PhysicalZip {get; set;}
       public decimal Premium {get; set;}

    } 
}
