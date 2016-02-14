using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mobilehome.insure.Models;
using MobileHome.Insure.Service;

namespace MobileHome.Insure.Web.Controllers
{
    public class OwnedController : Controller
    {
        //
        // GET: /Home/

        private  ServiceFacade _serviceFacade;

        public OwnedController()
        {
            _serviceFacade = new Service.ServiceFacade();
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
