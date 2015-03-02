using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileHome.Insure.Service;

namespace MobileHome.Insure.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private readonly IServiceFacade _serviceFacade;

        //public HomeController(IServiceFacade serviceFacade)
        //{
        //    _serviceFacade = serviceFacade;
        //}

        public ActionResult Index()
        {
           // _serviceFacade.getStates();
            return View();
        }

    }
}
