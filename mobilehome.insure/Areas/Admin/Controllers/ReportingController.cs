using mobilehome.insure.Models.JQDataTable;
using MobileHome.Insure.Model;
using MobileHome.Insure.Model.DTO;
using MobileHome.Insure.Service.Master;
using MobileHome.Insure.Service.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mobilehome.insure.Areas.Admin.Controllers
{
    [Authorize]
    public class ReportingController : Controller
    {
        IMasterServiceFacade _masterServiceFacade;
        IRentalServiceFacade _rentalServiceFacade;

        public ReportingController()
        {
            _masterServiceFacade = new MasterServiceFacade();
            _rentalServiceFacade = new RentalServiceFacade();
        }

        #region Order Report
        //
        // GET: /Admin/Reporting/
        public ActionResult Order()
        {
            return View();
        }

        public ActionResult SearchOrder(string startDate, string endDate)
        {
            return Json(_rentalServiceFacade.GetListOrder(startDate, endDate), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Customer Report
        public ActionResult Customer()
        {
            return View();
        }

        public ActionResult LoadingCustomer(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;
            int searchRecordCount = 0;

            var customers = GenericFilterHelper<Customer>.GetFilteredRecords(
                runTimeMethod: new MobileHome.Insure.Service.Rental.RentalServiceFacade().GetCustomers,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns("desc"),
                totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "FirstName", "LastName", "Phone", "Email", "Address", "Zip", "City" });

            return Json(new JQueryDataTablesResponse<Customer>(
                items: customers,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }

        public ActionResult LoadCustomer(JQueryDataTablesModel jQueryDataTablesModel)
        {
            SearchParameter param = new SearchParameter();
            param.SearchColumn = new List<string> { "Id", "FirstName",
                                                "LastName",                                               
                                                "Phone",
                                                "Email",
                                                "Address",
                                                 "Zip",
                                                 "City"
                                            };
            param.SearchColumnValue = jQueryDataTablesModel.sSearch_;
            param.StartIndex = jQueryDataTablesModel.iDisplayStart;
            param.PageSize = jQueryDataTablesModel.iDisplayLength;
            List<Customer> sourceData = new MobileHome.Insure.Service.Rental.RentalServiceFacade().GetCustomers(param);

            var customers = GenericFilterHelper<Customer>.GetFilteredRecords(
                sourceData: sourceData,
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns("desc"),
                totalRecordCount: param.TotalRecordCount,
                searchString: jQueryDataTablesModel.sSearch,
                isSearch: param.IsFilterValue,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id", "FirstName", "LastName", "Phone", "Email", "Address", "Zip", "City" });

            return Json(new JQueryDataTablesResponse<Customer>(
                items: customers,
                totalRecords: param.TotalRecordCount,
                totalDisplayRecords: param.SearchedCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }
        #endregion

        #region Park Report
        public ActionResult Park()
        {
            return View();
        }

        public ActionResult SearchPark(string parkName, int? stateId, string zipCode)
        {            
            return Json(_masterServiceFacade.GetListParks(parkName, (stateId.HasValue ? stateId.Value : 0), zipCode), JsonRequestBehavior.AllowGet);
        }
        
        //public ActionResult LoadingPark(JQueryDataTablesModel jQueryDataTablesModel)
        //{
        //    int totalRecordCount = 0;
        //    int searchRecordCount = 0;

        //    var parkDtos = GenericFilterHelper<ParkDto>.GetFilteredRecords(
        //        runTimeMethod: GetParkDtos,
        //        startIndex: jQueryDataTablesModel.iDisplayStart,
        //        pageSize: jQueryDataTablesModel.iDisplayLength,
        //        sortedColumns: jQueryDataTablesModel.GetSortedColumns("desc"),
        //        totalRecordCount: out totalRecordCount,
        //        searchRecordCount: out searchRecordCount,
        //        searchString: jQueryDataTablesModel.sSearch,
        //        searchColumnValues: jQueryDataTablesModel.sSearch_,
        //        properties: new List<string> { "Id", "ParkName", "PhysicalAddress", "PhysicalZip", "PhysicalCity", "State", "TotalOwnRentals" });

        //    return Json(new JQueryDataTablesResponse<ParkDto>(
        //        items: parkDtos,
        //        totalRecords: totalRecordCount,
        //        totalDisplayRecords: searchRecordCount,
        //        sEcho: jQueryDataTablesModel.sEcho));
        //}

        private List<ParkDto> GetParkDtos()
        {
            return _masterServiceFacade.GetParks().Select(p => new ParkDto
                                                                                {
                                                                                    Id = p.Id,
                                                                                    ParkName = p.ParkName,
                                                                                    PhysicalAddress = p.PhysicalAddress,
                                                                                    PhysicalZip = p.PhysicalZip,
                                                                                    PhysicalCity = p.PhysicalCity,
                                                                                    State = p.PhysicalState.Name,
                                                                                    TotalOwnRentals = p.Customers.Where(c => c.ParkId == p.Id).Count()
                                                                                }).ToList();
        }


        #endregion

        public ActionResult PotentialCustomers()
        {
            return View();
        }

        #region Premium Report
        public ActionResult PremiumReport()
        {
            return View();
        }

        public ActionResult SearchPremium(int? stateId, string zipCode, string startDate, string endDate)
        {
            return Json(_rentalServiceFacade.GetListPremiums((stateId.HasValue ? stateId.Value : 0), zipCode, startDate, endDate), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult ParkSites()
        {
            return View();
        }
        public JsonResult LoadParkSites(JQueryDataTablesModel jQueryDataTablesModel)
        {
            SearchParameter param = new SearchParameter();
            param.SearchColumn = new List<string> { "Id", "ParkId", 
                                                "SiteNumber",
                                                "ParkName",
                                                "TenantFirstName",
                                                "TenantLastName",
                                                "CompanyName",
                                                "EffectiveDate",
                                                "ExpiryDate",
                                                "SiteRental",
                                                "Liability",
                                                "PersonalProperty",
                                                "Premium"
                                            };
            param.SearchColumnValue = jQueryDataTablesModel.sSearch_;
            param.StartIndex = jQueryDataTablesModel.iDisplayStart;
            param.PageSize = jQueryDataTablesModel.iDisplayLength;

            List<ParkSiteDto> result = _masterServiceFacade.GetParkSites(param);

            var parks = GenericFilterHelper<ParkSiteDto>.GetFilteredRecords(
                sourceData: result, //Updating bcos, on/off feature has to implement
                startIndex: jQueryDataTablesModel.iDisplayStart,
                pageSize: jQueryDataTablesModel.iDisplayLength,
                sortedColumns: jQueryDataTablesModel.GetSortedColumns(string.Empty),
                totalRecordCount: param.TotalRecordCount,
                isSearch: param.IsFilterValue,
                searchString: jQueryDataTablesModel.sSearch,
                searchColumnValues: jQueryDataTablesModel.sSearch_,
                properties: new List<string> { "Id","ParkId",
                                                "SiteNumber",
                                                "ParkName",
                                                "TenantFirstName",
                                                "TenantLastName",
                                                "CompanyName",
                                                "EffectiveDate",
                                                "ExpiryDate",
                                                "SiteRental",
                                                "Liability",
                                                "PersonalProperty",
                                                "Premium"
                                            });

            return Json(new JQueryDataTablesResponse<ParkSiteDto>(
                items: parks,
                totalRecords: param.TotalRecordCount,
                totalDisplayRecords: param.SearchedCount,
                sEcho: jQueryDataTablesModel.sEcho));
        }

    }
}