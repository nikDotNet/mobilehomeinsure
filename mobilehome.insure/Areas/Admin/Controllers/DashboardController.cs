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

        public ActionResult Index()
        {
            if (TempData["IsLoggedIn"].ToString() != "true")
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            return View();
        }

        public ActionResult Logout()
        {
            TempData["IsLoggedIn"] = "false";
            return RedirectToAction("Index", "Login", new { area = "" });
        }

    }
}
