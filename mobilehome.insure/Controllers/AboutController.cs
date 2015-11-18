using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mobilehome.insure.Controllers
{
    [Authorize]
    public class AboutController : Controller
    {
        //
        // GET: /About/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LowestCost()
        {
            return View();
        }
        public ActionResult ExpertAdvice()
        {
            return View();
        }
        public ActionResult HavingPeace()
        {
            return View();
        }
	}
}