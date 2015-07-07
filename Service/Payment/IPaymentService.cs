using MobileHome.Insure.Model.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Service.Payment
{
   public interface IPaymentService
    {
        PaymentResponse RequestPayment(PaymentRequest requestPayment);

    }
}
