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

        public ActionResult LoadCustomer(JQueryDataTablesModel jQueryDataTablesModel)
        {
            SearchParameter param = new SearchParameter();
            param.SearchColumn = new List<string> { "Id", "FirstName",
                                                "LastName",
                                                "FirstName2",
                                                "LastName2",
                                                "Phone",
                                                "Email",
                                                "Address",
                                                 "Zip"
                                            };
            param.SearchColumnValue = jQueryDataTablesModel.sSearch_;
            param.StartIndex = jQueryDataTablesModel.iDisplayStart;
            param.PageSize = jQueryDataTablesModel.iDisplayLength;
            List<Customer> sourceData = _rentalServiceFacade.GetCustomers(param);

            var customers = GenericFilterHelper<Customer>.GetFilteredRecords(
                sourceData: sourceData,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns("desc"),
                totalRecordCount: param.TotalRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                isSearch: param.IsFilterValue,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "FirstName", "LastName", "FirstName2",
                    "LastName2", "Phone", "Email", "Address", "Zip" });

            return Json(new JQueryDataTablesResponse<Customer>(
                items: customers,
                totalRecords: param.TotalRecordCount,
                totalDisplayRecords: param.SearchedCount,
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
                properties: new List<string> { "Id", "ProposalNumber", "PersonalProperty", "Liability",
                    "Premium", "EffectiveDate", "NoOffInstallments", "SendLandLord" });

            return Json(new JQueryDataTablesResponse<QuoteDto>(
                items: quotes,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }

        public ActionResult LoadPolicy(JQueryDataTablesModel jQueryDataTablesModel)
        {
            SearchParameter param = new SearchParameter();
            param.SearchColumn = new List<string> { "Id",
                                                "InsuredName",
                                                "InsuredAddress",
                                                "InsuredPhone",
                                                "InsuredEmail",                                                
                                                "ProposalNumber",
                                                "PersonalProperty",
                                                "Liability",
                                                "Premium",
                                                "EffectiveDate",
                                                "NoOffInstallments",
                                                "SendLandLord"
                                            };
            param.SearchColumnValue = jQueryDataTablesModel.sSearch_;
            param.StartIndex = jQueryDataTablesModel.iDisplayStart;
            param.PageSize = jQueryDataTablesModel.iDisplayLength;
            List<QuoteDto> sourceData = _rentalServiceFacade.GetPolicies(param);
            var quotes = GenericFilterHelper<QuoteDto>.GetFilteredRecords(
                sourceData: sourceData,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(string.Empty),
                totalRecordCount: param.TotalRecordCount,
                isSearch: param.IsFilterValue,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id",
                                                "InsuredName",
                                                "InsuredAddress",
                                                "InsuredPhone",
                                                "InsuredEmail",                                                
                                                "ProposalNumber",
                                                "PersonalProperty",
                                                "Liability",
                                                "Premium",
                                                "EffectiveDate",
                                                "NoOffInstallments",
                                                "SendLandLord" });

            return Json(new JQueryDataTablesResponse<QuoteDto>(
                items: quotes,
                totalRecords: param.TotalRecordCount,
                totalDisplayRecords: param.SearchedCount,
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
                infotrntim = creationDate.ToShortTimeString(),
                infopmtinstfee = quoteObject != null && quoteObject.InstallmentFee.HasValue 
                    ? quoteObject.InstallmentFee.Value 
                    : 0,
                infopmtprocfee = quoteObject != null && quoteObject.ProcessingFee.HasValue 
                    ? quoteObject.ProcessingFee.Value
                    : 0,
                infopmtamttotal = quoteObject != null && quoteObject.Premium.HasValue 
                    ? quoteObject.Premium.Value 
                    : 0,
                pmtamttoday = quoteObject != null && quoteObject.PremiumChargedToday.HasValue 
                    ? quoteObject.PremiumChargedToday.Value 
                    : 0,
                infonoofremainingpmt = quoteObject != null && quoteObject.Payments.Any() 
                    ? quoteObject.NoOfInstallments.Value - 1
                    : 0

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

        public ActionResult LoadQuote(JQueryDataTablesModel jQueryDataTablesModel)
        {
            SearchParameter param = new SearchParameter();
            param.SearchColumn = new List<string> { "Id", "CustomerName",
                                                "PersonalProperty",
                                                "Liability",
                                                "Premium",
                                                "EffectiveDate",
                                                "NoOffInstallments",
                                                "SendLandLord"
                                            };
            param.SearchColumnValue = jQueryDataTablesModel.sSearch_;
            param.StartIndex = jQueryDataTablesModel.iDisplayStart;
            param.PageSize = jQueryDataTablesModel.iDisplayLength;
            List<QuoteDto> sourceData = _rentalServiceFacade.GetQuotes(param);

            var quotes = GenericFilterHelper<QuoteDto>.GetFilteredRecords(
                sourceData: sourceData,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(string.Empty),
                totalRecordCount: param.TotalRecordCount,
                isSearch: param.IsFilterValue,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "CustomerName", "PersonalProperty",
                    "Liability", "Premium", "EffectiveDate", "NoOfInstallments", "SendLandLord" });

            return Json(new JQueryDataTablesResponse<QuoteDto>(
                items: quotes,
                totalRecords: param.TotalRecordCount,
                totalDisplayRecords: param.SearchedCount,
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
        public ActionResult LoadPayment(JQueryDataTablesModel jQueryDataTablesModel)
        {
            SearchParameter param = new SearchParameter();
            param.SearchColumn = new List<string> { "Id", "TransactionId",
                                                "ResponseCode",
                                                "ApprovalCode",
                                                "CreationDate",
                                                "Amount"
                                            };
            param.SearchColumnValue = jQueryDataTablesModel.sSearch_;
            param.StartIndex = jQueryDataTablesModel.iDisplayStart;
            param.PageSize = jQueryDataTablesModel.iDisplayLength;
            List<Payment> sourceData = _rentalServiceFacade.GetPayments(param);

            var payments = GenericFilterHelper<MobileHome.Insure.Model.Payment>.GetFilteredRecords(
                sourceData: sourceData,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(string.Empty),
                totalRecordCount: param.TotalRecordCount,
                isSearch: param.IsFilterValue,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "TransactionId", "ResponseCode", "ApprovalCode", "CreationDate", "Amount" });

            return Json(new JQueryDataTablesResponse<MobileHome.Insure.Model.Payment>(
                items: payments,
                totalRecords: param.TotalRecordCount,
                totalDisplayRecords: param.SearchedCount,
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
