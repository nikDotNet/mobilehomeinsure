using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model.PaymentGateway
{
    public class PaymentRequest
    {
        public PaymentRequest()
        {
            TransactionType = "SALE";
            Method = "ProcessTranx";
            Terms = "Y";
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public string Terms { get; set; }
        public string Method { get; set; }
        public string TransactionType { get; set; }
        public string CreditCardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public decimal Amount { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string Zip { get; set; }
        public string InvoiceNumber { get; set; }
        public string CSC { get; set; }
    }
}
