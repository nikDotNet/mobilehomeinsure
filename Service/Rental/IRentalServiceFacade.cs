using MobileHome.Insure.Model;
using MobileHome.Insure.Model.DTO;
using MobileHome.Insure.Model.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Service.Rental
{
    public interface IRentalServiceFacade
    {
        int saveCustomerInformation(string FirstName, string LastName, string FirstName2, string LastName2, string Email, string Password, string Address, int StateId, string City, string Zip, string Phone, int parkId);
        decimal generateQuote(DateTime EffectiveDate, decimal PersonalProperty, decimal Deductible, decimal Liability, int CustomerId, int NoOfInstallments, bool SendLandlord, ref int quoteId, out string ProposalNo, out decimal premiumChargedToday);
        void saveQuote(Quote quoteObj);
        int generateInvoice(decimal amount, int customerId, int quoteId);
        bool SendLandlord(int quoteId, bool SendLandlord);
        bool saveInvoice(int PaymentId, string ResponseCode, string TransactionId, string ApprovalCode, string approvalMessage, string ErrorMessage, DateTime creationDate);

        List<Customer> GetCustomers();
        List<Customer> GetCustomers(SearchParameter searchParam);

        Customer GetCustomerById(int Id);

        List<QuoteDto> GetQuotes(SearchParameter searchParam);
        List<QuoteDto> GetQuotes();

        Quote GetQuoteById(int Id);

        List<QuoteDto> GetPolicies();
        List<QuoteDto> GetPolicies(SearchParameter searchParam);

        Model.Payment GetPolicyReceiptById(int quoteId);

        List<MobileHome.Insure.Model.Payment> GetPayments();
        List<MobileHome.Insure.Model.Payment> GetPayments(SearchParameter searchParam);

        bool SaveCustomerInfo(int id, string delType);

        bool SaveParkNotify(ParkNotify notify);

        List<Customer> OwnRentalCustomerByParkId(int parkId);

        List<OrderDto> GetListOrder(string startDate, string endDate);

        List<OrderDto> GetListPremiums(int stateId, string zipCode, string startDate, string endDate);
    }
}
