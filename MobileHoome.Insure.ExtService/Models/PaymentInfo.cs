using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MobileHoome.Insure.ExtService.Models
{
    public class PaymentInfo
    {
        [DataMember]
        public string polnbr {get; set;}
        [DataMember]
        public string cmpcod {get; set;}
        [DataMember]
        public string pmttyp {get; set;}
        [DataMember]
        public string billtype { get; set; }
        [DataMember]
        public string pmtid { get; set; }
        [DataMember]
        public string pmtamt { get; set; }
        [DataMember]
        public string procfee { get; set; }
        [DataMember]
        public string latefee { get; set;}
        [DataMember]
        public string reinsfee { get; set; }
        [DataMember]
        public string payopt { get; set; }
        [DataMember]
        public string trndat {get; set;}
        [DataMember]
        public string trntim {get; set;}
        [DataMember]
        public string payee {get; set;}
        [DataMember]
        public string nsffee { get; set; }
        [DataMember]
        public string prcemp { get; set; }
        [DataMember]
        public string polact { get; set; }

    }
}
