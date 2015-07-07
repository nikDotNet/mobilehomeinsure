using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Model.PaymentGateway
{
   public class PaymentResponse
    {
       public string ReponseCode {get; set;}
       public string TransactionId {get; set;}
       public string ApprovalCode {get; set;}
       public string ApprovalMessage {get; set;}
       public string ErrorMessage { get; set; }
       public string AVSResponse {get; set;}
       public string CSCResponse {get; set;}

    }
}
