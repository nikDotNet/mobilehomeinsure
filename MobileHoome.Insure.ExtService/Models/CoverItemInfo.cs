using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MobileHoome.Insure.ExtService.Models
{
    [System.Xml.Serialization.XmlRoot(ElementName = "cov_item")]
    [DataContract(Name = "cov_item")]
    public class CoverItemInfo
    {
        public CoverItemInfo()
        {

        }

        public CoverItemInfo(CoverType coverType)
        {
            this.cov_name = coverType.ToString();
        }
        //public CoverType PolicyCoverType { get; set; }

        [DataMember]
        public string cov_name { get; set; }

        [DataMember]
        public string deductible { get; set; }

        [DataMember]
        public string limit { get; set; }

        [DataMember]
        public string written_premium { get; set; }

        [DataMember]
        public string inforce_premium { get; set; }
    }

    [DataContract]
    public enum CoverType
    {
        [EnumMember]
        persprop,
        [EnumMember]
        deductible,
        [EnumMember]
        lou,
        [EnumMember]
        liability,
        [EnumMember]
        liab,
        [EnumMember]
        medpay,
        [EnumMember]
        thirdpartydesignee,

    }
}
