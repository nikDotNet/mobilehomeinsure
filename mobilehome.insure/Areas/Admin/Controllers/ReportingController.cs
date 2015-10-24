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

        #endregion

        #region Park Report
        public ActionResult Park()
        {
            return View();
        }

        public ActionResult SearchPark(string parkName, int? stateId, string zipCode)
        {
            return Json(_masterServiceFacade.GetListParks(parkName, (stateId.HasValue ? stateId.Value : 0), zipCode), JsonRequestBehavior.AllowGet);
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