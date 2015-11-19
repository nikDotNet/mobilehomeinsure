using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mobilehome.insure.Models;
using MobileHome.Insure.Service;
using System.Web.Security;

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

        [Authorize]
        public ActionResult Index()
        {
           // _serviceFacade.getStates();
            ViewBag.ContactSent = false;
            return View();
        }

        [Authorize]
        public ActionResult AboutUs()
        {
            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.ContactSent = false;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ContentResult Contact(ContactViewModel objContact)
        {
            try
            {
                objContact.message = objContact.message + " <br /> <br /> <hr /> Please click Reply to reply to the sender.";
                _serviceFacade.sendMail(objContact.senderEmail,"info@mobilehome.insure",objContact.subject, objContact.message);
                return Content("success");
            }
            catch (Exception ex)
            {
                return Content("Failed");
            }
        }

        [HttpGet]
        public ActionResult UploadPart()
        {
            return View();   
        }

        [HttpPost]
        public ActionResult UploadPart(HttpPostedFile file)
        {
            if(file != null)
            {
                file.SaveAs("");
            }
            return View();
        }

        [Authorize]
        public ActionResult TermsAndConditions()
        {
            return View();
        }

        [Authorize]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Unsubscribe(string user)
        {
            ViewBag.success = _serviceFacade.Unsubscribe(user);
            return View();
        }


    }
}
