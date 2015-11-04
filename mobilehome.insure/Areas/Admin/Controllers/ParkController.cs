using mobilehome.insure.Areas.Admin.Models;
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

namespace mobilehome.insure.Areas.Admin.Controllers
{
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
            //var model = new ParkViewModel();
            //model.Parks = _masterServiceFacade.GetParks();

            //return View(model);

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
                            ////TODO: for save file on server
                            //var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));
                            //string pathString = Path.Combine(originalDirectory.ToString(), "imagepath");
                            //var fileName1 = Path.GetFileName(file.FileName);
                            //bool isExists = Directory.Exists(pathString);

                            //if (!isExists)
                            //    System.IO.Directory.CreateDirectory(pathString);

                            //var path = string.Format("{0}\\{1}", pathString, file.FileName);
                            //file.SaveAs(path);



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

        //public ActionResult Export(bool? export) //any format like CSV/PDF/EXCEL etc
        //{
        //    var exportList = _masterServiceFacade.GetParks();
        //    if (export.HasValue)
        //        exportList.ExportCSV("ParksList_" + DateTime.Now.ToString());

        //    return View(exportList);
        //}

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
                                                "OwnerState.Name", 
                                                "PhysicalZip", 
                                                "IsActive" 
                                            });

            return Json(new JQueryDataTablesResponse<ParkDto>(
                items: parks,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }
    }
}
