using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MobileHoome.Insure.ExtService.Models
{
    [System.Xml.Serialization.XmlRoot(ElementName = "prdinfo")]
    [DataContract(Name = "prdinfo")]
    public class PropertyDealerInfo
    {
        [DataMember]
        public int agent { get; set; }

        [DataMember]
        public string agtsb1 { get; set; }

        [DataMember]
        public int biltyp { get; set; }
    }
}
