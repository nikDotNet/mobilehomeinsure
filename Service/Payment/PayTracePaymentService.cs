using MobileHome.Insure.Model.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MobileHome.Insure.Service.Payment
{
   public class PayTracePaymentService : IPaymentService
    {
        public PaymentResponse RequestPayment(PaymentRequest requestPayment)
        {
            PaymentResponse response = new PaymentResponse();
            
            
            requestPayment.Username =  ConfigurationManager.AppSettings["PayTraceUsername"];
            requestPayment.Password = ConfigurationManager.AppSettings["PayTracePassword"];
            requestPayment.Url = ConfigurationManager.AppSettings["PayTraceGatewayUrl"];
            requestPayment.Method = "ProcessTranx";
            requestPayment.TransactionType = "Sale";
            requestPayment.Terms = "Y";
            

            WebClient wClient = new WebClient();

            //process a keyed transaction
            String sRequest = "PARMLIST=" + System.Web.HttpUtility.UrlEncode
                ("UN~" + requestPayment.Username +"|PSWD~"+ requestPayment.Password +
                "|TERMS~"+ requestPayment.Terms+"|METHOD~"+ requestPayment.Method +"|TRANXTYPE~"+ requestPayment.TransactionType +
                "|CC~"+ requestPayment.CreditCardNumber +"|EXPMNTH~"+ requestPayment.ExpiryMonth+"|EXPYR~"+ requestPayment.ExpiryYear+
                "|AMOUNT~"+ requestPayment.Amount +"|BADDRESS~"+requestPayment.BillingAddressLine1+"|BADDRESS~"+requestPayment.BillingAddressLine2 +
                "|BZIP~"+ requestPayment.Zip +"|INVOICE~"+ requestPayment.InvoiceNumber +"|");

            //process a swiped transaction
            //String sRequest = "PARMLIST=" + Server.UrlEncode("UN~demo123|PSWD~demo123|TERMS~Y|METHOD~ProcessTranx|TRANXTYPE~Sale|SWIPE~%B4012881888818888^Demo/Customer^1212101001020001000000701000000?;4012881888818888=12121010010270100001?|AMOUNT~1.00|BADDRESS~1234|BADDRESS~1234 Main Street|BZIP~97201|INVOICE~1234|");

            wClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            string sResponse = "";
            try
            {
                sResponse = wClient.UploadString(requestPayment.Url, sRequest);
            

                string strERROR = "";
                string strAPPCODE = "";
                string strTRANSACTIONID = "";

                string[] name_value_pairs = sResponse.Split('|');
                foreach (string row in name_value_pairs)
                {
                    string[] values = row.Split('~');
                    if (values.Length == 2)
                    {
                        if (values[0] == "ERROR")
                        {
                             strERROR = strERROR + values[1];
                        }
                        else if (values[0] == "APPCODE")
                        {
                            response.ApprovalCode = values[1];
                            strAPPCODE = values[1];
                        }
                        else if (values[0] == "TRANSACTIONID")
                        {
                            response.TransactionId = values[1];
                            strTRANSACTIONID = values[1];
                        }
                        //can continue to grab other response variables as needed
                    } //if (values.Length == 2) {
                } //foreach (string row in name_value_pairs) {

                if (strERROR != "")
                {
                    response.ErrorMessage = "<br>The following error occurred: " + strERROR + ".<br>";
                }
                else if (strAPPCODE != "")
                {
                  response.ApprovalMessage =   "<br>Transaction ID, " + strTRANSACTIONID + ", was approved with approval code " + strAPPCODE + ".<br>";
                }
                else
                {
                    response.ApprovalMessage = "<br>Transaction ID, " + strTRANSACTIONID + ", was NOT approved.<br>";
                }

                
            }

            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}
