using mobilehome.insure.Areas.Admin.Helpers;
using mobilehome.insure.Areas.Admin.Models;
using mobilehome.insure.Models.JQDataTable;
using MobileHome.Insure.Model;
using MobileHome.Insure.Service.Master;
using System;
using System.Web.Mvc;
using mobilehome.insure.Models.JQDataTable;

namespace mobilehome.insure.Areas.Admin.Controllers
{
    public class ParkController : Controller
    {
        private IMasterServiceFacade _masterServiceFacade;

        public ParkController()
        {
            this._masterServiceFacade = new MasterServiceFacade();
        }

        public ActionResult Index()
        {
            var model = new ParkViewModel();
            model.Parks = _masterServiceFacade.GetParks();

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            ParkViewModel model = null;
            if (id.HasValue)
            {
                model = new ParkViewModel();
                model.States = _masterServiceFacade.GetStates();
                model.CurrentPark = _masterServiceFacade.GetParkById(id.Value);
            }
            else
            {
                model = new ParkViewModel();
                model.CurrentPark = new Park { IsActive = true };
                model.States = _masterServiceFacade.GetStates();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ParkViewModel model)
        {
            try
            {
                _masterServiceFacade.SavePark(model.CurrentPark);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("Park", "Master", new { area = "admin" });
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var park = new Park();
                park.Id = id;
                _masterServiceFacade.SavePark(park, true);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }

            return RedirectToAction("Park", "Master", new { area = "admin" });
        }

        public ActionResult Export(bool? export) //any format like CSV/PDF/EXCEL etc
        {
            var exportList = _masterServiceFacade.GetParks();
            if (export.HasValue)
                exportList.ExportCSV("ParksList_" + DateTime.Now.ToString());

            return View(exportList);
        }

        //[HttpPost]
        public JsonResult Loading(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;
            int searchRecordCount = 0;

            var parks = GenericFilterHelper<Park>.GetFilteredRecords(
                runTimeMethod: _masterServiceFacade.GetParks,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength, sortedColumns: jQueryDataTablesModel.GetSortedColumns(),
                totalRecordCount: out totalRecordCount, searchRecordCount: out searchRecordCount, searchString: jQueryDataTablesModel.sSearch);

            return Json(new JQueryDataTablesResponse<Park>(
                items: parks,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }
    }
}
