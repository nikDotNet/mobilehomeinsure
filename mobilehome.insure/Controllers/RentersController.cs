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
using mobilehome.insure.Areas.Admin.Models;
using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Rental;
using mobilehome.insure.Helper.Constants;
using System.Configuration;

namespace MobileHome.Insure.Web.Controllers
{
    [Authorize]
    public class RentersController : Controller
    {
        //
        // GET: /Home/

        private IRentalServiceFacade _serviceFacade;
        private IPaymentService _paymentServiceFacade;
        private readonly MasterServiceFacade _masterServiceFacade;
        private readonly IServiceFacade _generalFacade;
        public RentersController()
        {
            _serviceFacade = new RentalServiceFacade();
            _paymentServiceFacade = new PayTracePaymentService();
            _masterServiceFacade = new MasterServiceFacade();
            _generalFacade = new ServiceFacade();
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
                                                    model.FirstName, model.LastName, model.FirstName2, model.LastName2, model.Email,
                                                    model.Password, model.Address,
                                                    model.StateId, model.City,
                                                    model.Zip, model.Phone, model.ParkId, model.SiteNumber);

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
            string proposalNumber = string.Empty;
            decimal premiumChargedToday, installmentFee, processingFee, totalChargedToday;
            var itemLia = GetLiabilities().Find(l => l.Id == Convert.ToInt32(model.Liability));
            model.Liability = itemLia != null ? Convert.ToDecimal(itemLia.Text.Replace("$", "")) : model.Liability;

            var itemPProperty = GetPersonalProperties().Find(l => l.Id == Convert.ToInt32(model.PersonalProperty));
            model.PersonalProperty = itemPProperty != null ? Convert.ToDecimal(itemPProperty.Text.Replace("$", "")) : model.PersonalProperty;

            model.Premium = _serviceFacade.generateQuote(model.EffectiveDate, model.PersonalProperty, model.Deductible, model.Liability, customerId, model.NumberOfInstallments, model.SendLandlord, ref quoteId, out proposalNumber, out premiumChargedToday, out installmentFee, out processingFee, out totalChargedToday);

            model.PremiumChargedToday = premiumChargedToday;
            TempData["QuoteId"] = quoteId;
            TempData["Premium"] = model.Premium;
            TempData["PremiumChargedToday"] = model.PremiumChargedToday;

            TempData["ProposalNumber"] = proposalNumber;
            TempData.Keep();

            return Json(new { Premium = model.Premium, QuoteId = quoteId, InstallmentFee = installmentFee.ToString("c"), ProcessingFee = processingFee.ToString("c"), TotalChargedToday = totalChargedToday.ToString("c") });
        }

        [HttpPost]
        public ActionResult _Step2LandLord(int QuoteId, bool SendLandlord)
        {
            return Json(_serviceFacade.SendLandlord(QuoteId, SendLandlord));
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
            string proposalNumber = TempData["ProposalNumber"].ToString();
            Quote quoteObject = _serviceFacade.GetQuoteById(quoteId);
            model.Amount = quoteObject.TotalChargedToday.Value;
            int InvoiceNumber = _serviceFacade.generateInvoice(model.Amount, customerId, quoteId);
            Customer customerObject = _serviceFacade.GetCustomerById(customerId);
            quoteObject.ProposalNumber = "";
            

            PaymentRequest request = new PaymentRequest
            {
                CreditCardNumber = model.CreditCardNumber,
                ExpiryMonth = model.ExpiryMonth,
                ExpiryYear = model.ExpiryYear,
                Amount = model.Amount,
                BillingAddressLine1 = model.BillingAddressLine1,
                BillingAddressLine2 = model.BillingAddressLine2,
                Zip = model.Zip,
                InvoiceNumber = InvoiceNumber.ToString(),
                CSC = model.CSC
            };

            PaymentResponse paymentResponse = _paymentServiceFacade.RequestPayment(request);
            DateTime creationDate = DateTime.Now;
            bool success = _serviceFacade.saveInvoice(InvoiceNumber, paymentResponse.ReponseCode, paymentResponse.TransactionId, paymentResponse.ApprovalCode, paymentResponse.ApprovalMessage, paymentResponse.ErrorMessage, creationDate);
            if (success && (paymentResponse.Successfull.HasValue && paymentResponse.Successfull.Value) && string.IsNullOrEmpty(paymentResponse.ErrorMessage))
            {
                ViewBag.Success = true;
                _serviceFacade.GeneratePolicy(quoteObject);
                SaveParkSite(quoteId, customerObject, quoteObject);
                var result = new MobileHoome.Insure.ExtService.SendPaymentServiceForAegis();
                bool suc = result.makePayment(quoteObject.ProposalNumber.ToString(), customerObject.FirstName + " " + customerObject.LastName, paymentResponse.TransactionId.ToString(), (quoteObject.PremiumChargedToday.Value + Convert.ToDecimal(quoteObject.InstallmentFee.Value)).ToString(), quoteObject.NoOfInstallments.ToString(), quoteObject.CreationDate.Value);

                ViewBag.CustomerEmail = customerObject.Email;
                var rtn = new
                {
                    infoName = customerObject.FirstName + " " + customerObject.LastName,
                    infoAddress1 = customerObject.Address,
                    infoAddress2 = "",
                    infoCity = customerObject.City,
                    infoState = customerObject.State.Name,
                    infoZipCode = customerObject.Zip,
                    infoPhone = customerObject.Phone,
                    infoEmail = customerObject.Email,
                    infopolnbr = quoteObject.ProposalNumber,
                    infocopcod = "Aegis",
                    infopmtid = paymentResponse.TransactionId,
                    infopmtamttoday = model.Amount.ToString("c"),
                    infopmtprocfee = Convert.ToDecimal(quoteObject.ProcessingFee).ToString("c"),
                    infopmtinstfee = Convert.ToDecimal(quoteObject.InstallmentFee).ToString("c"),
                    infopmtamttotal = Convert.ToDecimal(quoteObject.Premium).ToString("c"),
                    infonoofremainingpmt = quoteObject.NoOfInstallments - 1,
                    infopayopt = Constants.InstallmentList[quoteObject.NoOfInstallments.Value],
                    infotrndat = creationDate.ToShortDateString(),
                    infotrntim = creationDate.ToShortTimeString()
                };
                TempData.Clear();

                return Json(rtn, JsonRequestBehavior.AllowGet);
            }
            else
                TempData.Keep();

            return Json("Failed");
        }

        private void SaveParkSite(int quoteId, Customer customerObject, Quote quoteObject)
        {   
            ParkSite parkSite = new ParkSite()
            {
                TenantFirstName = customerObject.FirstName,
                TenantLastName = customerObject.LastName,
                PhysicalCity = customerObject.Park.PhysicalCity,
                PhysicalStateId = customerObject.Park.PhysicalStateId,
                PhysicalZip = customerObject.Park.PhysicalZip,
                //Park = customerObject.Park,
                ParkId = customerObject.ParkId,
                //Quote = quoteObject,
                QuoteId = quoteId,
                //State = customerObject.State
                TenantEmail = customerObject.Email,
                TenantPhoneNumber = customerObject.Phone,
                SiteNumber = customerObject.SiteNumber
            };

            _masterServiceFacade.SaveParkSite(parkSite);
        }

        [HttpPost]
        public ContentResult SendReceiptOnEmail(string body, string customerEmail)
        {

            body = @"<style>#m-info{background:#fff;margin-bottom:30px}dd,dl{display:block}<style>#m-info{padding:40px 30px 30px}#page-header{padding:14px 0 15px}dd,dl,dt,li,ol,ul{margin:0;padding:0}#page-header h3 span{font-size:21px;color:#fff;background:#666;line-height:60px;display:table;margin:0 auto -30px;padding:0 20px}#page-header h3{border-bottom:1px solid #e6e6e6;font-weight:300;font-family:Roboto,Sans-serif}dl{-webkit-margin-before:1em;-webkit-margin-after:1em;-webkit-margin-start:0;-webkit-margin-end:0}dt{font-weight:700}dd,dt{line-height:1.42857143}dd{-webkit-margin-start:40px}</style> " + body;
            body = body.Replace(System.Environment.NewLine, "");
            _generalFacade.sendMail(ConfigurationManager.AppSettings["OrdersEmail"], customerEmail, "Receipt", body, null, true);
            return Content("Success");
        }

        public ActionResult FindZip(int zip)
        {
            var parks = _masterServiceFacade.FindParkByZip(zip);
            if (parks != null)
                parks.Add(new Park() { ParkName = "My Park is not listed", Id = 0, PhysicalAddress = "", PhysicalCity = "" });
            return Json(
                    new
                    {
                        Result = (parks == null ? null : parks),
                        Message = (parks == null ? string.Format("Unable to find Park(s) at Zip: {0}", zip) : string.Empty)
                    }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetParkDetail(int id)
        {
            Park model = null;
            if (id > 0)
            {
                model = _masterServiceFacade.GetParkById(id);
            }

            return Json(new
            {
                ParkName = model.ParkName,
                PhysicalAddress = model.PhysicalAddress,
                PhysicalCity = model.PhysicalCity,
                PhysicalStateAbbr = model.PhysicalState.Abbr,
                PhysicalStateId = model.PhysicalStateId,
                PhysicalState = model.PhysicalState.Name,
                Zip = model.PhysicalZip,
                PhysicalCountry = model.PhysicalCounty,
                OfficePhone = model.OfficePhone
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

        [HttpGet]
        public ActionResult Notify(string Zip)
        {
            NotifyViewModel model = new NotifyViewModel();
            model.Zip = Zip;
            return View(model);
        }

        [HttpPost]
        public ActionResult Notify(NotifyViewModel model)
        {
            ParkNotify request = new ParkNotify
            {
                Name = model.Name,
                Zip = model.Zip,
                Email = model.Email
            };


            bool success = _serviceFacade.SaveParkNotify(request);
            ViewBag.success = success;
            if (!success)
                return View(model);
            else
                return View();
        }
    }
}
