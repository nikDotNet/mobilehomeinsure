using mobilehome.insure.Helper.Constants;
using mobilehome.insure.Models.JQDataTable;
using MobileHome.Insure.Model;
using MobileHome.Insure.Service.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using mobilehome.insure.Helper.Extensions;
using MobileHome.Insure.Model.DTO;

namespace mobilehome.insure.Areas.Admin.Controllers
{
    [System.Web.Mvc.Authorize]
    public class CustomerController : Controller
    {
        IRentalServiceFacade _rentalServiceFacade;

        public CustomerController()
        {
            _rentalServiceFacade = new RentalServiceFacade();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadingCustomer(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;
            int searchRecordCount = 0;

            var customers = GenericFilterHelper<Customer>.GetFilteredRecords(
                runTimeMethod: _rentalServiceFacade.GetCustomers,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns("desc"),
                totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "FirstName", "LastName", "FirstName2", "LastName2", "Phone", "Email", "Address", "Zip" });

            return Json(new JQueryDataTablesResponse<Customer>(
                items: customers,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }


        public ActionResult Policy()
        {
            return View();
        }

        public ActionResult LoadingPolicy(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;
            int searchRecordCount = 0;

            var quotes = GenericFilterHelper<QuoteDto>.GetFilteredRecords(
                runTimeMethod: _rentalServiceFacade.GetPolicies,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(string.Empty),
                totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "ProposalNumber", "PersonalProperty", "Liability", "Premium", "EffectiveDate", "NoOffInstallments", "SendLandLord" });

            return Json(new JQueryDataTablesResponse<QuoteDto>(
                items: quotes,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }

        public ActionResult PolicyReceipt(int id)
        {
            var _serviceFacade = new RentalServiceFacade();
            MobileHome.Insure.Model.Rental.Quote quoteObject = _serviceFacade.GetQuoteById(id);
            Customer customerObject = null;
            DateTime creationDate = DateTime.Now;

            if (quoteObject.CustomerId.HasValue)
                customerObject = _serviceFacade.GetCustomerById(quoteObject.CustomerId.Value);
            MobileHome.Insure.Model.Payment paymentResponse = _serviceFacade.GetPolicyReceiptById(quoteObject.Id);

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
                infopmtamt = paymentResponse.Amount,
                infopayopt = Constants.InstallmentList[quoteObject.NoOfInstallments.Value],
                infotrndat = creationDate.ToShortDateString(),
                infotrntim = creationDate.ToShortTimeString()
            };

            //return Json(rtn, JsonRequestBehavior.AllowGet);

            return PartialView("~/areas/admin/views/customer/PolicyReceipt.cshtml", rtn.ToExpando());
        }

        public ActionResult Quote()
        {
            return View();
        }

        public ActionResult LoadingQuote(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;
            int searchRecordCount = 0;

            var quotes = GenericFilterHelper<QuoteDto>.GetFilteredRecords(
                runTimeMethod: _rentalServiceFacade.GetQuotes,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(string.Empty),
                totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "ProposalNumber", "PersonalProperty", "Liability", "Premium", "EffectiveDate", "NoOfInstallments", "SendLandLord" });

            return Json(new JQueryDataTablesResponse<QuoteDto>(
                items: quotes,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }

        public ActionResult Payment()
        {
            return View();
        }

        public ActionResult LoadingPayment(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;
            int searchRecordCount = 0;

            var payments = GenericFilterHelper<MobileHome.Insure.Model.Payment>.GetFilteredRecords(
                runTimeMethod: _rentalServiceFacade.GetPayments,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(string.Empty),
                totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "TransactionId", "ResponseCode", "ApprovalCode", "CreationDate", "Amount" });

            return Json(new JQueryDataTablesResponse<MobileHome.Insure.Model.Payment>(
                items: payments,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }

        public ActionResult Delete(int id, string delType)
        {
            try
            {
                TempData["Success"] = _rentalServiceFacade.SaveCustomerInfo(id, delType);
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }

            return RedirectToAction((delType == "customer" ? "Index" : delType == "quote" ? "Quote" : "Payment"), "Customer", new { area = "admin" });
        }

        public ActionResult OwnRentalCustomerByParkId(int parkId)
        {
            var customers = _rentalServiceFacade.OwnRentalCustomerByParkId(parkId);
            return PartialView("~/areas/admin/views/customer/OwnRentalCustomer.cshtml", customers);
        }
    }
}
