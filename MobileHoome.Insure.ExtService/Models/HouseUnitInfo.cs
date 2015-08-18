using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MobileHoome.Insure.ExtService.Models
{
    [System.Xml.Serialization.XmlRoot(ElementName = "ho_unit")]
    [DataContract(Name = "ho_unit")]
    public class HouseUnitInfo
    {
        [DataMember]
        public string unitnbr { get; set; }

        [DataMember]
        public string make { get; set; }

        [DataMember]
        public string model { get; set; }

        [DataMember]
        public string modelyear { get; set; }

        [DataMember]
        public string constyear { get; set; }

        [DataMember]
        public string consttype { get; set; }

        [DataMember]
        public string serialnbr { get; set; }

        [DataMember]
        public string lngth { get; set; }

        [DataMember]
        public string width { get; set; }

        [DataMember]
        public string fireprotcd { get; set; }

        [DataMember]
        public string familycount { get; set; }

        [DataMember]
        public string locaddr1 { get; set; }

        [DataMember]
        public string locaddr2 { get; set; }

        [DataMember]
        public string loccity { get; set; }

        [DataMember]
        public string locstate { get; set; }

        [DataMember]
        public string loccountynb { get; set; }

        [DataMember]
        public string locterritory { get; set; }

        [DataMember]
        public string loczip { get; set; }

        [DataMember]
        public string ratingbase { get; set; }

        [DataMember]
        public string parkcode { get; set; }

        [DataMember]
        public string ftfmhyd { get; set; }

        [DataMember]
        public string milfmfde { get; set; }

        [DataMember]
        public string prefrisk { get; set; }

        [DataMember]
        public string protcode { get; set; }

        [DataMember]
        public string purcpric { get; set; }

        [DataMember]
        public string purcdate { get; set; }

        [DataMember]
        public string tiedown { get; set; }

        [DataMember]
        public string woodstov { get; set; }

        [DataMember]
        public string fndtncod { get; set; }
    }
}
