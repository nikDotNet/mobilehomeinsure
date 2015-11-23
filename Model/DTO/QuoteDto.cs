using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model.DTO
{
    public class QuoteDto
    {
        public int Id {get; set;}
        public string ProposalNumber { get; set; }
        public decimal? Liability { get; set; }
        public decimal? PersonalProperty { get; set; }
        public decimal? Premium { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int? NoOfInstallments { get; set; }
        public bool SendLandLord { get; set; }
    }
}
