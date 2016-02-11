using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MobileHoome.Insure.ExtService.Models
{
     [System.Xml.Serialization.XmlRoot(ElementName = "ae_thirdpartydesignee")]
        [DataContract(Name = "ae_thirdpartydesignee")]
        public class ThirdPartyDesignee
        {
            [DataMember]
            public string name { get; set; }
            [DataMember]
            public string addr1 { get; set; }
            [DataMember]
            public string addr2 { get; set; }
            [DataMember]
            public string city { get; set; }
            [DataMember]
            public string state { get; set; }
            [DataMember]
            public string zip { get; set; }
            [DataMember]
            public string ai_desc { get; set; }
        }
}
