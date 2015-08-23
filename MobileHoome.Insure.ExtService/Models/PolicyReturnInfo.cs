using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MobileHoome.Insure.ExtService.Models
{
    [System.Xml.Serialization.XmlRoot(ElementName = "rtninfo")]
    [DataContract(Name = "rtninfo")]
    public class PolicyReturnInfo
    {
        [DataMember]
        public string returnc { get; set; }

        [DataMember]
        public string premwrit { get; set; }

        [DataMember]
        public string policynbr { get; set; }

        [DataMember]
        public string progmode { get; set; }

        [DataMember]
        public string effdate { get; set; }

        [DataMember]
        public string productcde { get; set; }

        [DataMember]
        public string lstate { get; set; }

        [DataMember]
        public string polterm { get; set; }
    }
}
