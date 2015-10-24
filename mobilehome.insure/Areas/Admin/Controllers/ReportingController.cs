using mobilehome.insure.Models.JQDataTable;
using MobileHome.Insure.Model;
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

        #region Order Report
        //
        // GET: /Admin/Reporting/
        public ActionResult Order()
        {
            return View();
        }

        public ActionResult SearchOrder(string startDate, string endDate)
        {
            return Json(_masterServiceFacade.GetListOrder(startDate, endDate), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Customer Report
        public ActionResult Customer()
        {
            return View();
        }

        public ActionResult SearchCustomer(string zipCode, string lastName)
        {
            return Json(_masterServiceFacade.GetListCustomers(zipCode, lastName), JsonRequestBehavior.AllowGet);
        }

        //public ActionResult LoadingCustomer(JQueryDataTablesModel jQueryDataTablesModel)
        //{
        //    int totalRecordCount = 0;
        //    int searchRecordCount = 0;

        //    var customers = GenericFilterHelper<Customer>.GetFilteredRecords(
        //        runTimeMethod: _masterServiceFacade.GetListCustomers,
        //        startIndex: jQueryDataTablesModel.iDisplayStart,
        //        pageSize: jQueryDataTablesModel.iDisplayLength,
        //        sortedColumns: jQueryDataTablesModel.GetSortedColumns("desc"),
        //        totalRecordCount: out totalRecordCount,
        //        searchRecordCount: out searchRecordCount,
        //        searchString: jQueryDataTablesModel.sSearch,
        //        searchColumnValues: jQueryDataTablesModel.sSearch_,
        //        properties: new List<string> { "Id", "FirstName", "LastName", "Phone", "Email", "Address", "Zip", "City", "StateName" });

        //    return Json(new JQueryDataTablesResponse<Customer>(
        //        items: customers,
        //        totalRecords: totalRecordCount,
        //        totalDisplayRecords: searchRecordCount,
        //        sEcho: jQueryDataTablesModel.sEcho));
        //}

        #endregion

        #region Park Report
        public ActionResult Park()
        {
            return View();
        }

        public ActionResult LoadingPark(JQueryDataTablesModel jQueryDataTablesModel)
        {
            return null;
        }
        #endregion

        public ActionResult PotentialCustomers()
        {
            return View();
        }

        #region Premium Report
        public ActionResult PremiumReport()
        {
            return View();
        }

        public ActionResult SearchPremium(int? stateId, string zipCode, string startDate, string endDate)
        {
            return Json(_masterServiceFacade.GetListPremiums((stateId.HasValue ? stateId.Value : 0), zipCode, startDate, endDate), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}