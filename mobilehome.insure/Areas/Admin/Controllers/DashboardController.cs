using mobilehome.insure.Areas.Admin.Models;
using MobileHome.Insure.Model;
using MobileHome.Insure.Model.DTO;
using MobileHome.Insure.Model.Rental;
using MobileHome.Insure.Service.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mobilehome.insure.Areas.Admin.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        //
        // GET: /Admin/Dashboard/
        IRentalServiceFacade _rentalFacade;
        public DashboardController()
        {
            _rentalFacade = new RentalServiceFacade();
        }

        public ActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();
            DateTime sevenDaysPrior = DateTime.Now.AddDays(-7);
            List<Customer> customerList = _rentalFacade.GetCustomers(sevenDaysPrior);
            List<QuoteDto> policyList = _rentalFacade.GetPolicies(sevenDaysPrior);

            model.NoOfNewCustomersInLast1Week = customerList.Count();
            model.NoOfNewCustomersInLast1Day = customerList.Where(x => x.CreationDate > DateTime.Now.AddDays(-1)).Count();
            model.NoOfNewCustomersInLast3Days = customerList.Where(x => x.CreationDate > DateTime.Now.AddDays(-3)).Count();

            model.NoOfNewPoliciesGeneratedInLast1Week = policyList.Count();
            model.NoOfNewPoliciesGeneratedInLast1Day = policyList.Where(x => x.CreationDate > DateTime.Now.AddDays(-1)).Count();
            model.NoOfNewPoliciesGeneratedInLast3Days = policyList.Where(x => x.CreationDate > DateTime.Now.AddDays(-3)).Count();

            model.PremiumChargedInLast1Day = policyList.Where(x => x.CreationDate > DateTime.Now.AddDays(-1)).Select(x => x.TotalChargedToday).Sum();
            model.PremiumChargedInLast3Days = policyList.Where(x => x.CreationDate > DateTime.Now.AddDays(-3)).Select(x => x.TotalChargedToday).Sum();
            model.PremiumChargedInLast1Week = policyList.Select(x => x.TotalChargedToday).Sum();

            model.IsPaymentProcessorLive = true;
            model.IsAegisServiceLive = true;
            return View(model);
        }

        public ActionResult Logout()
        {
            TempData["IsLoggedIn"] = "false";
            return RedirectToAction("Index", "Login", new { area = "" });
        }

        

    }
}
