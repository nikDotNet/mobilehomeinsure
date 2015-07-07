﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mobilehome.insure.Models;
using MobileHome.Insure.Service;
using mobilehome.insure.Models.Rental;
using MobileHome.Insure.Service.Rental;
using MobileHome.Insure.Model.PaymentGateway;
using MobileHome.Insure.Service.Payment;

namespace MobileHome.Insure.Web.Controllers
{
    public class RentalController : Controller
    {
        //
        // GET: /Home/

        private IRentalServiceFacade _serviceFacade;
        private IPaymentService _paymentServiceFacade;

        public RentalController()
        {
            _serviceFacade = new RentalServiceFacade();
            _paymentServiceFacade = new PayTracePaymentService();
        }

        public ActionResult Index()
        {
            RentalViewModel model = new RentalViewModel();
            
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
            TempData["CustomerId"] = _serviceFacade.saveCustomerInformation(model.Name, model.Address, model.Phone, model.Email, model.Zip);

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


    }
}
