using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mobilehome.insure.Models;
using MobileHome.Insure.Service;

namespace MobileHome.Insure.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private  ServiceFacade _serviceFacade;

        public HomeController()
        {
            _serviceFacade = new Service.ServiceFacade();
        }

        public ActionResult Index()
        {
           // _serviceFacade.getStates();
            ViewBag.ContactSent = false;
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }


        [HttpPost]
        public ContentResult Contact(ContactViewModel objContact)
        {
            try
            {
                _serviceFacade.sendMail(objContact.senderName, objContact.senderEmail, objContact.subject, objContact.message);
                return Content("success");
            }
            catch (Exception ex)
            {
                return Content("Failed");
            }


        }


    }
}
