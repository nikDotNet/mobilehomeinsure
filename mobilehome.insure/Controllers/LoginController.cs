using mobilehome.insure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MobileHome.Insure.Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            if (model.Name == "demo" && model.Password == "MobileHomeDemo2015!")
            {
                FormsAuthentication.SetAuthCookie("admin", false);
                TempData["IsLoggedIn"] = "true";
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}
