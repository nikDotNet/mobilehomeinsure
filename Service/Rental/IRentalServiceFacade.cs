using MobileHome.Insure.Model;
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
        decimal generateQuote(DateTime EffectiveDate, decimal PersonalProperty, decimal Deductible, decimal Liability, int CustomerId, int NoOfInstallments,bool SendLandlord, ref int quoteId);

        int generateInvoice(decimal amount, int customerId, int quoteId);
        bool SendLandlord(int quoteId, bool SendLandlord);
        bool saveInvoice(int PaymentId, string ResponseCode, string TransactionId, string ApprovalCode, string approvalMessage, string ErrorMessage, DateTime creationDate);

        List<Customer> GetCustomers();

        Customer GetCustomerById(int Id);

        List<Quote> GetQuotes();

        Quote GetQuoteById(int Id);

        List<MobileHome.Insure.Model.Payment> GetPayments();

        bool SaveCustomerInfo(int id, string delType);

        bool SaveParkNotify(ParkNotify notify);

        List<Customer> OwnRentalCustomerByParkId(int parkId);
    }
}
