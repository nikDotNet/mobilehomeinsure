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

namespace mobilehome.insure.Areas.Admin.Controllers
{
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
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(),
                totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "FirstName", "LastName", "Phone", "Email", "Address", "Zip" });

            return Json(new JQueryDataTablesResponse<Customer>(
                items: customers,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }


        public ActionResult Quote()
        {
            return View();
        }

        public ActionResult LoadingQuote(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;
            int searchRecordCount = 0;

            var quotes = GenericFilterHelper<MobileHome.Insure.Model.Rental.Quote>.GetFilteredRecords(
                runTimeMethod: _rentalServiceFacade.GetQuotes,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(),
                totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "ProposalNumber", "PersonalProperty", "Liability", "Premium", "EffectiveDate", "NoOffInstallments" });

            return Json(new JQueryDataTablesResponse<MobileHome.Insure.Model.Rental.Quote>(
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
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(),
                totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "TransactionId", "ResponseCode", "ApprovalCode", "ApprovalMessage", "Amount" });

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
    }
}
