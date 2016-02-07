using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHoome.Insure.ExtService
{
    interface ISendPaymentService
    {
        bool makePayment(string policyNumber, string customerName, string paymentID, string paymentAmount, string numberOfInstallments, DateTime transcationDateTime);
    }
}
