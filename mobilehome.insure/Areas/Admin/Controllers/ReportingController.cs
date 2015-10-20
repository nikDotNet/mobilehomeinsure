﻿using mobilehome.insure.Models.JQDataTable;
using MobileHome.Insure.Model.DTO;
using MobileHome.Insure.Service.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mobilehome.insure.Areas.Admin.Controllers
{
    public class ReportingController : Controller
    {
        IMasterServiceFacade _masterServiceFacade;
        public ReportingController()
        {
            _masterServiceFacade = new MasterServiceFacade();
        }
        //
        // GET: /Admin/Reporting/
        public ActionResult Order()
        {
            return View();
        }
        public ActionResult LoadingOrder(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;
            int searchRecordCount = 0;

            var customers = GenericFilterHelper<OrderDto>.GetFilteredRecords(
                runTimeMethod: _masterServiceFacade.GetListOrder,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns("desc"),
                totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "OrderId", "ApprovalCode", "ApprovalMessage", "CreatedBy", "CreationDate", "ErrorMessage", "ResponseCode", "TransactionId", "RenterId", "CompanyId", "CompanyName", "ProposalNumber", "CustomerId", "CustomerName" });

            return Json(new JQueryDataTablesResponse<OrderDto>(
                items: customers,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }

        public ActionResult Customer()
        {
            return View();
        }
        public ActionResult Park()
        {
            return View();
        }
        public ActionResult PotentialCustomers()
        {
            return View();
        }
        public ActionResult PremiumReport()
        {
            return View();
        }
	}
}