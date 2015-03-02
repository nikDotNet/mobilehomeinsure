using MobileHome.Insure.Service.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileHome.Insure.Web.Models.Admin;


namespace mobilehome.insure.Areas.Admin.Controllers
{
    public class AppraisalController : Controller
    {
        //
        // GET: /Admin/Appraisal/
        private readonly AppraisalServiceFacade _serviceFacade;

        public AppraisalController()
        {
            _serviceFacade = new AppraisalServiceFacade();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgeFactor()
        {
            var model = new ListAgeFactorViewModel();
           var AgeFactor = _serviceFacade.GetAgeFactor();

           foreach (var item in AgeFactor)
           {
               model.ListAgeFactor.Add(item);
           }

           return View(model);
        }

        public ActionResult AreaFactor()
        {
            var model = new ListAreaFactorViewModel();
            var AreaFactor = _serviceFacade.GetAreaFactor();

            foreach (var item in AreaFactor)
            {
                model.ListAreaFactor.Add(item);
            }

            return View(model);
        }

        public ActionResult StateFactor()
        {
            var model = new ListStateFactorViewModel();
            var StateFactor = _serviceFacade.GetStateFactor();

            foreach (var item in StateFactor)
            {
                model.ListStateFactor.Add(item);
            }

            return View(model);
        }

        public ActionResult ManufacturerFactor()
        {
            var model = new ListManufacturerFactorViewModel();
            var ManufacturerFactor = _serviceFacade.GetManufacturerFactor();

            foreach (var item in ManufacturerFactor)
            {
                model.ListManufacturerFactor.Add(item);
            }

            return View(model);
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
