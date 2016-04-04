using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mobilehome.insure.Areas.Admin.Models
{
    public class ExecuteQuoteViewModel
    {
        public string infoName {get; set;}
        public string infoAddress1 { get; set; }
        public string infoAddress2 { get; set; }
        public string infoCity { get; set; }
        public string infoState { get; set; }   
        public string infoZipCode { get; set; }
        public string infoPhone { get; set; }
        public string infoEmail { get; set; }
        public string infopolnbr { get; set; }
        public string infocopcod { get; set; }
        public string infomodeofpay {get; set;}
        public string infopmtid { get; set; }
        public decimal infopmtamt {get; set;}
        public string infopayopt { get; set; }
        public string infotrndat { get; set; }
        public string infotrntim { get; set; }
        public decimal infopmtinstfee { get; set; }
        public decimal infopmtprocfee { get; set; }
        public decimal infopmtamttotal { get; set; }
        public decimal pmtamttoday { get; set; }
        public int infonoofremainingpmt { get; set; }
        public int infoQuoteId { get; set; }    
    }
}