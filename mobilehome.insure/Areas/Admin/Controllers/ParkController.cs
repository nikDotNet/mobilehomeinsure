using mobilehome.insure.Models.JQDataTable;
using MobileHome.Insure.Model;
using MobileHome.Insure.Model.DTO;
using MobileHome.Insure.Service;
using MobileHome.Insure.Service.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using MobileHome.Insure.Service.Rental;
using mobilehome.insure.Areas.Admin.Models;

namespace mobilehome.insure.Areas.Admin.Controllers
{
    [Authorize]
    public class ParkController : Controller
    {
        private IMasterServiceFacade _masterServiceFacade;
        private IServiceFacade _generalServiceFacade;
        private IRentalServiceFacade _rentalServiceFacade;

        public ParkController()
        {
            this._masterServiceFacade = new MasterServiceFacade();
            this._generalServiceFacade = new ServiceFacade();
            _rentalServiceFacade = new RentalServiceFacade();
        }

        public ActionResult Index()
        {
            return View();
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

        public ActionResult EditParkSite(int? id)
        {
            ParkSitesViewModel model = new ParkSitesViewModel();
            model.States = _masterServiceFacade.GetStates();
            model.Parks = _masterServiceFacade.GetParks();
            if (id.HasValue)
            {
                model.CurrentParkSite = _masterServiceFacade.GetParkSiteById(id.Value);
            }
            else
            {
                model.CurrentParkSite = new ParkSite { IsActive = true };
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditParkSite(ParkSitesViewModel model)
        {
            try
            {
                _masterServiceFacade.SaveParkSite(model.CurrentParkSite,false);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }
            return RedirectToAction("ParkSites", "Park", new { area = "admin" });
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

        public ActionResult DeleteParkSite(int id)
        {
            var parkSite = new ParkSite();
            parkSite.Id = id;
            _masterServiceFacade.SaveParkSite(parkSite, true);
            TempData["Success"] = true;

            return RedirectToAction("ParkSites", "Park", new { area = "admin" });
        }

        //it's also using for On/Off feature.
        public ActionResult OnOffPark(int id, bool isOnOrOff)
        {
            try
            {
                _masterServiceFacade.OnOrOffPark(id, isOnOrOff);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
            }

            return RedirectToAction("Park", "Master", new { area = "admin" });
        }

        //[HttpPost]
        public ActionResult Import()//HttpPostedFileBase csvFile = null)
        {
            bool isSavedSuccessfully = false;
            List<ImportCsvStatus> processFiles = null;
            bool isProcessed = true; bool isException = false;
            string errorMsg = string.Empty;
            int idx = 0;

            //string fName = "";
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    processFiles = new List<ImportCsvStatus>();
                    foreach (string fileName in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[fileName];
                        if (file != null && file.ContentLength > 0)
                        {
                            var records = mobilehome.insure.Helper.DataImport.ParkCsvImport.ParkImport(file.InputStream);
                            var parks = this._masterServiceFacade.GetParksWithOnOff();

                            var findDuplicateRec = records.Where(dup => parks.Any(chk => chk.ParkName.Trim().Equals(dup.ParkName.Trim(), StringComparison.OrdinalIgnoreCase))).ToList();
                            records.RemoveAll(rem => findDuplicateRec.Any(item => item.ParkName.Trim().Equals(rem.ParkName.Trim())));

                            if (records != null && records.Count > 0)
                                this._masterServiceFacade.SavePark(records);

                            processFiles.Add(new ImportCsvStatus
                            {
                                Id = ++idx,
                                FileName = file.FileName,
                                CountDuplicates = (findDuplicateRec != null && findDuplicateRec.Count() > 0) ? findDuplicateRec.Count() : 0,
                                ClassName = (findDuplicateRec != null && findDuplicateRec.Count() > 0) ? "bg-danger" : "bg-success",
                                Message = (findDuplicateRec == null || (findDuplicateRec != null && findDuplicateRec.Count() == 0) ? "Successfully Imported Parks" : "Find duplicate records")
                            });
                        }

                        isSavedSuccessfully = true;
                    }
                }
                catch (Exception ex)
                {
                    isSavedSuccessfully = false;
                    errorMsg = ex.Message;
                    isProcessed = false;
                    isException = !isProcessed;
                }
            }
            else
                isProcessed = false;


            if (isSavedSuccessfully)
            {
                return Json(new { Result = processFiles, Message = string.Empty, IsProcessed = isProcessed });
            }
            else if (!isProcessed && !isException)
            {
                return Json(new { Message = "file(s) not found for upload.", IsProcessed = isProcessed });
            }
            else
            {
                return Json(new { Message = string.Format("Error :{0}\t{1}", Environment.NewLine, errorMsg), IsProcessed = isProcessed });
            }
        }

        //[HttpPost]
        public JsonResult Loading(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;
            int searchRecordCount = 0;

            var parks = GenericFilterHelper<ParkDto>.GetFilteredRecords(
                runTimeMethod: _masterServiceFacade.GetListPark, //Updating bcos, on/off feature has to implement
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(string.Empty),
                totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "ParkName", //"OfficePhone", 
                                                "SpacesToRent", 
                                                "SpacesToOwn", 
                                                "PhysicalAddress", 
                                                "State", 
                                                "PhysicalZip", 
                                                "IsActive" 
                                            });

            return Json(new JQueryDataTablesResponse<ParkDto>(
                items: parks,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }

        public JsonResult LoadPark(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;
            int searchRecordCount = 0;

            SearchParameter param = new SearchParameter();
            param.SearchColumn = new List<string> { "Id", "ParkName", //"OfficePhone", 
                                                "SpacesToRent",
                                                "SpacesToOwn",
                                                "PhysicalAddress",
                                                "State",
                                                "PhysicalZip",
                                                "IsActive"
                                            };
            param.SearchColumnValue = jQueryDataTablesModel.sSearch_;
            param.StartIndex = jQueryDataTablesModel.iDisplayStart;
            param.PageSize = jQueryDataTablesModel.iDisplayLength;

            List<ParkDto> result = _masterServiceFacade.GetListPark(param);

            var parks = GenericFilterHelper<ParkDto>.GetFilteredRecords(
                sourceData: result, //Updating bcos, on/off feature has to implement
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(string.Empty),  
                totalRecordCount: param.TotalRecordCount,             
                searchString: jQueryDataTablesModel.sSearch,
                isSearch:param.IsFilterValue,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "ParkName", //"OfficePhone", 
                                                "SpacesToRent",
                                                "SpacesToOwn",
                                                "PhysicalAddress",
                                                "State",
                                                "PhysicalZip",
                                                "IsActive"
                                            });

            return Json(new JQueryDataTablesResponse<ParkDto>(
                items: parks,
                totalRecords: param.TotalRecordCount,
                totalDisplayRecords: param.SearchedCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }
        public JsonResult LoadParkSites(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;
            int searchRecordCount = 0;
            
            var parks = GenericFilterHelper<ParkSiteDto>.GetFilteredRecords(
                runTimeMethod: _masterServiceFacade.GetParkSites, //Updating bcos, on/off feature has to implement
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(string.Empty),
                totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "SiteNumber", 
                                                "ParkName", 
                                                "TenantFirstName", 
                                                "TenantLastName", 
                                                "PhysicalCity",
                                                "PhysicalState", 
                                                "PhysicalZip", 
                                                "Premium",
                                                "IsActive" 
                                            });

            return Json(new JQueryDataTablesResponse<ParkSiteDto>(
                items: parks,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }

        
        public ActionResult ParkSites()
        {
            return View();
        }

    }
}
