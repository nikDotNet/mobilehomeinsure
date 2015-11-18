using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mobilehome.insure.Controllers
{
    [Authorize]
    public class SupportController : Controller
    {
        //
        // GET: /Support/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StraightTalk()
        {
            return View();
        }
	}
}