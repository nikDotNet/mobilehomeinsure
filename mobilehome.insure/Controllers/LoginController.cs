using mobilehome.insure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            if (model.Name == "admin" && model.Password == "password")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            return View();
        }
    }
}
