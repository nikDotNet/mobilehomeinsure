using MobileHoome.Insure.ExtService.AegisRental;
using MobileHoome.Insure.ExtService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;

namespace MobileHoome.Insure.ExtService
{
    public class SendPaymentServiceForAegis : ISendPaymentService
    {
        public bool makePayment(string policyNumber, string customerName, string paymentID, string paymentAmount, string numberOfInstallments, DateTime transcationDateTime)
        {   
            PaymentInfo pmtInfo = new PaymentInfo
            {
                billtype = "DIRECT BILL",
                cmpcod = "1",
                payee = customerName,
                payopt = numberOfInstallments,
                pmtamt = paymentAmount,
                pmtid = paymentID,
                pmttyp = "SW",
                polnbr = policyNumber,
                trndat = (transcationDateTime.Month.ToString().Length == 1 ? "0" + transcationDateTime.Month.ToString() : transcationDateTime.Month.ToString())
                + (transcationDateTime.Day.ToString().Length == 1 ? "0" + transcationDateTime.Day.ToString() : transcationDateTime.Day.ToString())
                + transcationDateTime.Year.ToString(),
                trntim = transcationDateTime.Hour + ":" + transcationDateTime.Minute,
                latefee ="",
                nsffee = "",
                prcemp = "",
                polact = "",
                reinsfee = "",
                procfee = ""
            };

            XElement rootEle = new XElement("root", Helpers.Extensions.ToXml(pmtInfo).Nodes(), new XElement("proc-by"), new XElement("proc-status"));

            //Call service and get the result with Premium
            ServiceSoapClient sClient = new ServiceSoapClient(ConfigurationManager.AppSettings["ServiceConfigName"]);
            sClient.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(rootEle.ToString());
            XmlNode xnode = doc.FirstChild;
            XmlNode result = sClient.QuotePolicy(ConfigurationManager.AppSettings["PasskeyForAegisService"], xnode, "PM", AstecProcessingMode.SubmitOverride);

            if (result != null)
            {
                var elements = result.SelectSingleNode("proc-status");
                if (elements != null)
                {
                    return true;
                }

            }

            return false;                         
        }
    }
}
