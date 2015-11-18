using MobileHome.Insure.Service.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileHome.Insure.Web.Models.Admin;
using MobileHome.Insure.Model.Appraisal;
using mobilehome.insure.Areas.Admin.Models;
using MobileHome.Insure.Service.Master;
using mobilehome.insure.Areas.Admin.Helpers;


namespace mobilehome.insure.Areas.Admin.Controllers
{
    [Authorize]
    public class AppraisalController : Controller
    {
        //
        // GET: /Admin/Appraisal/
        private readonly AppraisalServiceFacade _serviceFacade;
        private readonly MasterServiceFacade  _masterFacade;

        public AppraisalController()
        {
            _serviceFacade = new AppraisalServiceFacade();
            _masterFacade = new MasterServiceFacade();
        }
        public ActionResult Index()
        {
            return View();
        }



       


        #region AgeFactor
        public ActionResult AgeFactor(bool? exportCsv)
        {
            var model = new ListAgeFactorViewModel();
           var AgeFactor = _serviceFacade.GetAgeFactor();

           foreach (var item in AgeFactor)
           {
               model.ListAgeFactor.Add(item);
           }

           if (exportCsv.HasValue)
               model.ListAgeFactor.ExportCSV("AgeFactor_" + DateTime.Now.ToString());

           return View(model);
        }


        [HttpGet]
        public ActionResult _partialAgeFactor(int? id)
        {
            ListAgeFactorViewModel model = new ListAgeFactorViewModel();
            
            if(id.HasValue)
            {
                model.ageFactorObj = _serviceFacade.GetAgeFactorById(id.Value);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult _partialAgeFactor(ListAgeFactorViewModel objFactorViewModel)
        {
            try
            {
                
                    _serviceFacade.saveAgeFactor(objFactorViewModel.ageFactorObj);
                    TempData["Success"] = true;
                
                

            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("AgeFactor");

        }

        
        public ActionResult deleteAgeFactor (int id)
        {
            try
            {
                var objAgeFactor = new MobileHome.Insure.Model.Appraisal.AgeFactor();
                objAgeFactor.Id = id;
                _serviceFacade.saveAgeFactor(objAgeFactor, true);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("AgeFactor");
        }

#endregion

        #region AreaFactor

        public ActionResult AreaFactor(bool? exportCsv)
        {
            var model = new ListAreaFactorViewModel();
            var AreaFactor = _serviceFacade.GetAreaFactor();

            foreach (var item in AreaFactor)
            {
                model.ListAreaFactor.Add(item);
            }

            if (exportCsv.HasValue)
                model.ListAreaFactor.ExportCSV("AreaFactor_" + DateTime.Now.ToString());

            return View(model);
        }


        [HttpGet]
        public ActionResult _partialAreaFactor(int? id)
        {
            ListAreaFactorViewModel model = new ListAreaFactorViewModel();
            
            if(id.HasValue)
            {
                model.areaFactorObj = _serviceFacade.getAreaFactorById(id.Value);
            }
                        
            return View(model);
        }

        [HttpPost]
        public ActionResult _partialAreaFactor(ListAreaFactorViewModel areaFactorViewModel)
        {
            try
            {
                 _serviceFacade.saveAreaFactor(areaFactorViewModel.areaFactorObj);
                 TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("AreaFactor");
        }

        public ActionResult deleteAreaFactor(int id)
        {
            try
            {
                var objAreaFactor = new MobileHome.Insure.Model.Appraisal.AreaFactor();
                objAreaFactor.Id = id;
                _serviceFacade.saveAreaFactor(objAreaFactor, true);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("AreaFactor");
        }


#endregion

        #region state factor
        public ActionResult StateFactor(bool? exportCsv)
        {
            var model = new ListStateFactorViewModel();
            var StateFactor = _serviceFacade.GetStateFactor();

            foreach (var item in StateFactor)
            {
                model.ListStateFactor.Add(item);
            }

            if (exportCsv.HasValue)
                model.ListStateFactor.ExportCSV("StateFactor_" + DateTime.Now.ToString());

            return View(model);
        }


        [HttpGet]
        public ActionResult _partialStateFactor(int? id)
        {
            ListStateFactorViewModel model = new ListStateFactorViewModel();
            model.lstStates = _masterFacade.GetStates();
            
            if(id.HasValue)
            {
                model.stateFactorObj = _serviceFacade.getStateFactorById(id.Value);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult _partialStateFactor(ListStateFactorViewModel objStateFactorViewModel)
        {
            try
            {
                _serviceFacade.saveStateFactor(objStateFactorViewModel.stateFactorObj);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            
            return RedirectToAction("StateFactor");

        }


        public ActionResult deleteStateFactor(int id)
        {
            try
            {
                var objStateFactor = new MobileHome.Insure.Model.Appraisal.StateFactor();
                objStateFactor.Id = id;
                _serviceFacade.saveStateFactor(objStateFactor, true);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("StateFactor");
        }


        #endregion

        #region options factor

        public ActionResult OptionsFactor(bool? exportCsv)
        {
            var model = new ListOptionsFactorViewModel();
            var optionsFactor = _serviceFacade.getOptionFactors();
            model.lstOptionFactors = optionsFactor;

            if (exportCsv.HasValue)
                model.lstOptionFactors.ExportCSV("OptionsFactor_"+ DateTime.Now.ToString());

            return View(model);
        }


        [HttpGet]
        public ActionResult _partialOptions(int? Id)
        {
            OptionsFactorViewModel model = new OptionsFactorViewModel();
            if(Id.HasValue)
            {
                model.optionFactorObject= _serviceFacade.getOptionFactorsById(Id.Value);

            }

            model.lstOptionsType = _masterFacade.getOptionTypes();
            model.lstManufacturers = _masterFacade.GetManufacturer();
            model.lstStates = _masterFacade.GetStates();

            return View(model);
        }

        [HttpPost]
        public ActionResult _partialOptions(OptionsFactorViewModel optionsFactorModelObj)
        {
            try
            {
                
                _serviceFacade.saveOptionsFactor(optionsFactorModelObj.optionFactorObject);
                TempData["Success"] = true;
            }
            catch(Exception ex)
            {
                TempData["Success"] = false;
            }

            return  RedirectToAction("OptionsFactor");

        }


        public ActionResult deletePartialOptions(int id)
        {
            try
            {
                var objPartialOptions = new MobileHome.Insure.Model.Appraisal.OptionsFactor();
                objPartialOptions.Id = id;
                _serviceFacade.saveOptionsFactor(objPartialOptions, true);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("OptionsFactor");
        }



        #endregion

        #region manufacturer factor
        public ActionResult ManufacturerFactor(bool? exportCsv)
        {
            var model = new ListManufacturerFactorViewModel();
            model.ListManufacturerFactor = _serviceFacade.GetManufacturerFactor();

            if (exportCsv.HasValue)
                model.ListManufacturerFactor.ExportCSV("ManufacturerFactor_" + DateTime.Now.ToString());

           return View(model);
        }


        [HttpGet]
        public ActionResult _partialManufacturerFactor(int? id)
        {
            ListManufacturerFactorViewModel model = new ListManufacturerFactorViewModel();
            model.lstManufacturers = _masterFacade.GetManufacturer();
            if (id.HasValue)
                model.objManufacturerFactor = _serviceFacade.getManufacturerFactorById(id.Value);
            return View(model);

        }
         
        [HttpPost]
        public ActionResult _partialManufacturerFactor(ListManufacturerFactorViewModel manufacturerViewModelObj)
        {
            try
            {
                _serviceFacade.saveManufacturerFactor(manufacturerViewModelObj.objManufacturerFactor);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("ManufacturerFactor");
        }

        public ActionResult deleteManufacturerFactor(int id)
        {
            try
            {
                var objManufacturerFactor = new MobileHome.Insure.Model.Appraisal.ManufacturerFactor();
                objManufacturerFactor.Id = id;
                _serviceFacade.saveManufacturerFactor(objManufacturerFactor, true);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("ManufacturerFactor");
        }

        #endregion


    }
}
