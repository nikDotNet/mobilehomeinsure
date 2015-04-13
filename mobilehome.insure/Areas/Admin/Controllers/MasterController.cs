using MobileHome.Insure.Service.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileHome.Insure.Web.Models.Admin;
using MobileHome.Insure.Service.Master;
using mobilehome.insure.Areas.Admin.Helpers;


namespace mobilehome.insure.Areas.Admin.Controllers
{
    public class MasterController : Controller
    {
        //
        // GET: /Admin/Appraisal/
        private readonly MasterServiceFacade _serviceFacade;

        public MasterController()
        {
            _serviceFacade = new MasterServiceFacade();
        }
        public ActionResult Index()
        {
            return View();
        }

        #region State
        public ActionResult State(bool? exportCsv)
        {
            var model = new ListStateViewModel();
            var State = _serviceFacade.GetStates();

            foreach (var item in State)
            {
                model.ListState.Add(item);
            }

            if (exportCsv.HasValue)
                model.ListState.ExportCSV("StatesList_" + DateTime.Now.ToString());

            return View(model);
        }


        [HttpGet]
        public ActionResult _partialEditState(int? id)
        {
            ListStateViewModel model = new ListStateViewModel();
            
            if (id.HasValue)
                model.stateObj = _serviceFacade.GetStateById(id.Value);

            return View(model);

        }

        [HttpPost]
        public ActionResult _partialEditState(ListStateViewModel stateViewModelObj)
        {
            try
            {
                _serviceFacade.saveState(stateViewModelObj.stateObj);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("State");
        }


        public ActionResult deleteState(int id)
        {
            try
            {
                var objState = new MobileHome.Insure.Model.State();
                objState.Id = id;
                _serviceFacade.saveState(objState, true);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("State");
        }




        #endregion

        #region Manufacturer

        public ActionResult Manufacturer(bool? exportCsv)
        {
            var model = new ListManufacturerViewModel();
            var Manufacturer = _serviceFacade.GetManufacturer();

            foreach (var item in Manufacturer)
            {
                model.ListManufacturer.Add(item);
            }

            if (exportCsv.HasValue)
                model.ListManufacturer.ExportCSV("ManufacturersList_" + DateTime.Now.ToString());

            return View(model);
        }

        [HttpGet]
        public ActionResult _partialEditManufacturer(int? id)
        {
            ListManufacturerViewModel model = new ListManufacturerViewModel();
            
        //    model.ListManufacturer = _serviceFacade.GetManufacturer();
            if (id.HasValue)
                model.manufacturerObj = _serviceFacade.GetManufacturerById(id.Value);
            else
                model.manufacturerObj = new MobileHome.Insure.Model.Manufacturer();

            return View(model);

        }

        [HttpPost]
        public ActionResult _partialEditManufacturer(ListManufacturerViewModel manufacturerViewModelObj)
        {
            try
            {
                _serviceFacade.saveManufacturer(manufacturerViewModelObj.manufacturerObj);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("Manufacturer");
        }


        public ActionResult deleteManufacturer(int id)
        {
            try
            {
                var objManufacturer = new MobileHome.Insure.Model.Manufacturer();
                objManufacturer.Id = id;
                _serviceFacade.saveManufacturer(objManufacturer, true);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("Manufacturer");
        }

        #endregion





    }
}
