using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Service.Rental
{
    public interface IRentalServiceFacade
    {
        int saveCustomerInformation(string Name, string Address, string Phone, string email, string Zip);
        decimal generateQuote(DateTime EffectiveDate, decimal PersonalProperty, decimal Deductible, decimal Liability,int CustomerId, int NoOfInstallments, ref int quoteId);

        int generateInvoice(decimal amount, int customerId, int quoteId);

        bool saveInvoice(int PaymentId, string ResponseCode, string TransactionId, string ApprovalCode, string approvalMessage, string ErrorMessage);

    
    }
}
