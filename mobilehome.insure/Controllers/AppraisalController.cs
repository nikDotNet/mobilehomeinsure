﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileHome.Insure.Service.Appraisal;
using MobileHome.Insure.Web.Models.Appraisal;
using MobileHome.Insure.Service.Master;

namespace MobileHome.Insure.Web.Controllers
{
    public class AppraisalController : Controller
    {
        //
        // GET: /Appraisal/

        private readonly AppraisalServiceFacade _serviceFacade;
        private readonly MasterServiceFacade _masterServiceFacade;

        public AppraisalController()
        {
            _serviceFacade = new AppraisalServiceFacade();
            _masterServiceFacade = new MasterServiceFacade();
        }


        [HttpGet]
        public ActionResult Index()
        {
            MobileHomeAppraisalViewModel model = new MobileHomeAppraisalViewModel();
            model.ManufacturerList = _masterServiceFacade.GetManufacturer();
            model.StateList = _masterServiceFacade.GetStates();
            model.OptionsType = _masterServiceFacade.getOptionTypes();
            return View(model);
        }

        //[HttpPost]
        //public ActionResult Index(MobileHomeAppraisalViewModel model)
        //{
        //    model.EstimatedValue = _serviceFacade.calculateAppraisalValue(model.State, model.Manufacturer, model.Length, model.Width, model.ModelYear);
        //    model.ManufacturerList = _serviceFacade.getManufacturers();
        //    model.StateList = _serviceFacade.getStates();
        //    return View(model);
        //}

        public JsonResult GetEstimatedValue(int StateId, int ManafacturerId, int Length, int width, int year, List<int> options, decimal? BrickLinearFootage, decimal? VinylLinearFootage, decimal? AreaOfDeckPorche, decimal? AreaOfAdditions)
        {
            decimal EstimatedValue = _serviceFacade.calculateAppraisalValue(StateId, ManafacturerId, Length, width, year, options, BrickLinearFootage, VinylLinearFootage, AreaOfDeckPorche,AreaOfAdditions);
            return Json(EstimatedValue.ToString("c"), JsonRequestBehavior.AllowGet);
        }
    }
}
