using MobileHome.Insure.Service.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileHome.Insure.Web.Models.Admin;


namespace mobilehome.insure.Areas.Admin.Controllers
{
    public class MasterController : Controller
    {
        //
        // GET: /Admin/Appraisal/
        private readonly AppraisalServiceFacade _serviceFacade;

        public MasterController()
        {
            _serviceFacade = new AppraisalServiceFacade();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult State()
        {
            var model = new ListStateViewModel();
            var State = _serviceFacade.GetState();

            foreach (var item in State)
            {
                model.ListState.Add(item);
            }

            return View(model);
        }

        public ActionResult Manufacturer()
        {
            var model = new ListManufacturerViewModel();
            var Manufacturer = _serviceFacade.GetManufacturer();

            foreach (var item in Manufacturer)
            {
                model.ListManufacturer.Add(item);
            }

            return View(model);
        }


      



    }
}
