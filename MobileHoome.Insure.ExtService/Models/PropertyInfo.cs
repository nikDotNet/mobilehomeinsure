using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MobileHoome.Insure.ExtService.Models
{
    [System.Xml.Serialization.XmlRoot(ElementName = "pplinfo")]
    [DataContract(Name = "pplinfo")]
    public class PropertyInfo
    {
        public PropertyInfo()
        {
            this.ppltyp = "I";
        }

        [DataMember]
        public string ppltyp { get; set; }

        [DataMember]
        public int pplnbr { get; set; }

        [DataMember]
        public string firstname { get; set; }

        [DataMember]
        public string lastname { get; set; }

        [DataMember]
        public string middleinit { get; set; }

        [DataMember]
        public string insaddr1 { get; set; }

        [DataMember]
        public string insaddr2 { get; set; }

        [DataMember]
        public string inscity { get; set; }

        [DataMember]
        public string insstate { get; set; }

        [DataMember]
        public string inscounty { get; set; }

        [DataMember]
        public string insctycode { get; set; }

        [DataMember]
        public string inszip { get; set; }

        [DataMember]
        public string insdob { get; set; }

        [DataMember]
        public string insfin { get; set; }

        [DataMember]
        public string insssn { get; set; }

        [DataMember]
        public string insphone { get; set; }

        [DataMember]
        public string insoccup { get; set; }

        [DataMember]
        public string reltoapp { get; set; }
    }
}
