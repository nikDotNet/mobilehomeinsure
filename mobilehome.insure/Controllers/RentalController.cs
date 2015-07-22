using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mobilehome.insure.Models;
using MobileHome.Insure.Service;
using mobilehome.insure.Models.Rental;
using MobileHome.Insure.Service.Rental;
using MobileHome.Insure.Model.PaymentGateway;
using MobileHome.Insure.Service.Master;
using MobileHome.Insure.Service.Payment;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace MobileHome.Insure.Web.Controllers
{
    public class RentalController : Controller
    {
        //
        // GET: /Home/

        private IRentalServiceFacade _serviceFacade;
        private IPaymentService _paymentServiceFacade;
        private readonly MasterServiceFacade _masterServiceFacade;
        public RentalController()
        {
            _serviceFacade = new RentalServiceFacade();
            _paymentServiceFacade = new PayTracePaymentService();
            _masterServiceFacade = new MasterServiceFacade();
        }

        public ActionResult Index()
        {
            RentalViewModel model = new RentalViewModel();
            model.customer.States = _masterServiceFacade.GetStates();
            model.quote.Liabilities = GetLiabilities();
            model.quote.PersonalProperties = GetPersonalProperties();
            return View(model);
        }


        [HttpGet]
        public ActionResult _Step1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _Step1(RentalViewModel.Customer model)
        {
            TempData["CustomerId"] = _serviceFacade.saveCustomerInformation(
                                                    model.FirstName, model.LastName, model.Email,
                                                    model.Password, model.Address,
                                                    model.StateId, model.City,
                                                    model.Zip, model.Phone);

            return Json(true);
        }

        [HttpGet]
        public ActionResult _Step2()
        {

            return View();
        }

        [HttpPost]
        public ActionResult _Step2(RentalViewModel.Quote model)
        {
            int quoteId = TempData["QuoteId"] == null ? 0 : Convert.ToInt32(TempData["QuoteId"]); ;
            int customerId = TempData["CustomerId"] == null ? 0 : Convert.ToInt32(TempData["CustomerId"]);

            var itemLia = GetLiabilities().Find(l => l.Id == Convert.ToInt32(model.Liability));
            model.Liability = itemLia != null ? Convert.ToDecimal(itemLia.Text.Replace("$", "")) : model.Liability;

            var itemPProperty = GetPersonalProperties().Find(l => l.Id == Convert.ToInt32(model.PersonalProperty));
            model.PersonalProperty = itemPProperty != null ? Convert.ToDecimal(itemPProperty.Text.Replace("$", "")) : model.PersonalProperty;

            model.Premium = _serviceFacade.generateQuote(model.EffectiveDate, model.PersonalProperty, model.Deductible, model.Liability, customerId, model.NumberOfInstallments, ref quoteId);
            TempData["QuoteId"] = quoteId;
            TempData["Premium"] = model.Premium;
            TempData.Keep();

            return Json(model.Premium);
        }

        [HttpGet]
        public ActionResult _Step3()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _Step3(RentalViewModel.Payment model)
        {
            int customerId = TempData["CustomerId"] == null ? 0 : Convert.ToInt32(TempData["CustomerId"]);
            int quoteId = TempData["QuoteId"] == null ? 0 : Convert.ToInt32(TempData["QuoteId"]);
            model.Amount = (decimal)TempData["Premium"];
            int InvoiceNumber = _serviceFacade.generateInvoice(model.Amount, customerId, quoteId);

            PaymentRequest request = new PaymentRequest
            {
                CreditCardNumber = model.CreditCardNumber,
                ExpiryMonth = model.ExpiryMonth,
                ExpiryYear = model.ExpiryYear,
                Amount = model.Amount,
                BillingAddressLine1 = model.BillingAddressLine1,
                BillingAddressLine2 = model.BillingAddressLine2,
                Zip = model.Zip,
                InvoiceNumber = InvoiceNumber.ToString()
            };


            PaymentResponse paymentResponse = _paymentServiceFacade.RequestPayment(request);

            bool success = _serviceFacade.saveInvoice(InvoiceNumber, paymentResponse.ReponseCode, paymentResponse.TransactionId, paymentResponse.ApprovalCode, paymentResponse.ApprovalMessage, paymentResponse.ErrorMessage);

            if (success)
            {
                ViewBag.Success = true;
                TempData.Clear();
                TempData["Success"] = "true";
            }
            else
                TempData.Keep();

            return Json("Success");
        }

        public ActionResult FindZip(int zip)
        {
            var parks = _masterServiceFacade.FindParkByZip(zip);

            return Json(
                    new
                    {
                        Result = (parks == null ? null : parks),
                        Message = (parks == null ? string.Format("Unable to find Park(s) at Zip: {0}", zip) : string.Empty)
                    }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private List<OptionListItem> GetLiabilities()
        {
            return new List<OptionListItem>() 
                {
                    new OptionListItem{Id=1, Text="$ 25,000"},
                    new OptionListItem{Id=2, Text="$ 50,000"}
                };
        }

        [NonAction]
        private List<OptionListItem> GetPersonalProperties()
        {
            return new List<OptionListItem>() 
                {
                    new OptionListItem{Id=1, Text="$ 15,000"},
                    new OptionListItem{Id=2, Text="$ 20,000"},
                    new OptionListItem{Id=3, Text="$ 25,000"}
                };
        }
    }
}
