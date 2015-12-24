using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileHome.Insure.DAL.EF;
using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Rental;
using MobileHome.Insure.Model.DTO;
using System.Data.Entity.SqlServer;
//using MobileHome.Insure.Service.Search;
//using MobileHome.Insure.Service.Repository;

namespace MobileHome.Insure.Service.Master
{
    public class MasterServiceFacade : IMasterServiceFacade
    {
        private mhappraisalContext _context;
        private mhRentalContext _rentalcontext;
        private IServiceFacade _genralServiceFacade;

        public MasterServiceFacade()
        {
            _context = new mhappraisalContext();            
            _genralServiceFacade = new ServiceFacade();
        }

        #region Manufacturers
        public List<Manufacturer> GetManufacturer()
        {
            return _context.Manufacturers.Where(x => x.isActive == true).ToList();
        }

        public Manufacturer GetManufacturerById(int id)
        {
            return _context.Manufacturers.Where(x => x.Id == id && x.isActive == true).SingleOrDefault();
        }

        public void saveManufacturer(Manufacturer manufacturerObj, bool toDelete = false)
        {
            if (manufacturerObj.Id != 0)
            {
                var existingObj = _context.Manufacturers.Where(x => x.Id == manufacturerObj.Id).SingleOrDefault();
                if (existingObj != null)
                {
                    if (toDelete)
                        existingObj.isActive = false;
                    else
                    {
                        existingObj.Name = manufacturerObj.Name;
                    }
                    _context.Entry(existingObj).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            else
            {
                manufacturerObj.CreatedBy = "admin";
                manufacturerObj.CreatedOn = DateTime.Now;
                manufacturerObj.isActive = true;
                _context.Manufacturers.Add(manufacturerObj);
                _context.SaveChanges();
            }
        }

        #endregion

        #region state
        public List<State> GetStates()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.States.AsNoTracking().Where(x => x.isActive == true).OrderBy(x => x.Name).ToList();
        }

        public State GetStateById(int id)
        {
            return _context.States.Where(x => x.Id == id && x.isActive == true).SingleOrDefault();
        }

        public void saveState(State stateObj, bool toDelete = false)
        {
            if (stateObj.Id != 0)
            {
                var existingObj = _context.States.Where(x => x.Id == stateObj.Id).SingleOrDefault();
                if (existingObj != null)
                {
                    if (toDelete)
                        existingObj.isActive = false;
                    else
                    {
                        existingObj.Name = stateObj.Name;
                        existingObj.Abbr = stateObj.Abbr;
                    }
                    _context.Entry(existingObj).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            else
            {
                stateObj.CreatedBy = "admin";
                stateObj.CreatedOn = DateTime.Now;
                stateObj.isActive = true;
                _context.States.Add(stateObj);
                _context.SaveChanges();
            }
        }

        #endregion

        #region OptionTypes
        public Dictionary<int, string> getOptionTypes()
        {
            Dictionary<int, string> optionTypes = new Dictionary<int, string>();
            var listOptionTypes = _context.OptionsTypes.Where(x => x.IsActive == true).ToList();

            foreach (var opt in listOptionTypes)
            {
                optionTypes.Add((int)opt.Id, opt.Name);
            }
            return optionTypes;
        }

        #endregion

        #region Park operation
        public List<Park> GetParks()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.Parks.AsNoTracking().Where(x => x.IsActive == true).ToList();
        }
        public List<Park> GetFirstFewParks()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.Parks.AsNoTracking().Where(x => x.IsActive == true)
                .OrderBy(x => x.Id)
                .Skip(1).Take(10)
                .ToList();
        }
        public List<Park> GetParks(string searchParam)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.Parks.AsNoTracking().Where(x => x.IsActive == true && x.ParkName.ToUpper().StartsWith(searchParam.ToUpper())).ToList();
        }
        public List<Park> GetParksWithOnOff()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.Parks.AsNoTracking().Where(x => x.IsActive == true).ToList();
        }

        public List<ParkDto> GetListPark()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            var items = _context.Parks.AsNoTracking().ToList();
            var state = _context.States.ToList();
            
            var rtnItems = items.Select(x => new ParkDto()
            {
                Id = x.Id,
                IsActive = x.IsActive,
                ParkName = x.ParkName,
                PhysicalAddress = x.PhysicalAddress,
                PhysicalStateId = x.PhysicalStateId,
                PhysicalZip = x.PhysicalZip,
                SpacesToOwn = x.SpacesToOwn,
                SpacesToRent = x.SpacesToRent,
                State = (x.PhysicalStateId != null || x.PhysicalStateId != 0) ? state.Where(y => y.Id == x.PhysicalStateId).SingleOrDefault().Abbr : ""
            }).ToList();
            return rtnItems;
        }
        private ParkDto GetSearchObject(SearchParameter searchParam)
        {
            ParkDto parkObj = new ParkDto();
            if (searchParam != null)
            {
                var isFilterValue = searchParam.SearchColumnValue.Any(e => !string.IsNullOrWhiteSpace(e));

                searchParam.IsFilterValue = isFilterValue;

                if ((searchParam.SearchColumn != null && searchParam.SearchColumn.Count > 0) &&
                    searchParam.SearchColumn.Count == searchParam.SearchColumnValue.Count - 1 && isFilterValue) // minus -1 means, skipping action column from search list
                {
                    var filterValueProp = new Dictionary<string, string>();
                    for (int idx = 0; idx < searchParam.SearchColumnValue.Count; idx++)
                    {
                        if (!string.IsNullOrWhiteSpace(searchParam.SearchColumnValue[idx]))
                        {
                            if (searchParam.SearchColumn[idx] == "Id") parkObj.Id = Convert.ToInt32(searchParam.SearchColumnValue[idx]);
                            else if (searchParam.SearchColumn[idx] == "ParkName") parkObj.ParkName = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "SpacesToRent") parkObj.SpacesToRent = Convert.ToInt32(searchParam.SearchColumnValue[idx]);
                            else if (searchParam.SearchColumn[idx] == "SpacesToOwn") parkObj.SpacesToOwn = Convert.ToInt32(searchParam.SearchColumnValue[idx]);
                            else if (searchParam.SearchColumn[idx] == "PhysicalAddress") parkObj.PhysicalAddress = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "State") parkObj.State = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "PhysicalZip") parkObj.PhysicalZip = Convert.ToInt32(searchParam.SearchColumnValue[idx]);
                            //else if (searchParam.SearchColumn[idx] == "IsActive") parkObj.IsActive = Convert.ToBoolean(searchParam.SearchColumnValue[idx]);                           

                        }
                    }                                                                                 
                }
                
            }
            return parkObj;
        }
        public List<ParkDto> GetListPark(SearchParameter searchParam)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            List<ParkDto> result = null;
            if (searchParam != null)
            {                
                List<Park> items = null;
                ParkDto ParkDto = GetSearchObject(searchParam);
                Int32 SpacesToRent = Convert.ToInt32(ParkDto.SpacesToRent);
                Int32 SpacesToOwn = Convert.ToInt32(ParkDto.SpacesToOwn);
                string ParkName = Convert.ToString(ParkDto.ParkName);
                string State = Convert.ToString(ParkDto.State);
                string PhysicalAddress = Convert.ToString(ParkDto.PhysicalAddress);

                if (!searchParam.IsFilterValue)
                {
                    searchParam.TotalRecordCount = _context.Parks.Count();
                    items = _context.Parks.Include("PhysicalState").
                    OrderBy(x => x.Id)
                    .Skip(searchParam.StartIndex).Take((searchParam.PageSize > 0 ? searchParam.PageSize : searchParam.TotalRecordCount)).
                    ToList();
                }
                else
                {
                    

                    items = _context.Parks.Include("PhysicalState").Where(m =>
                        (ParkDto.Id == 0 ? 1 == 1 : m.Id == ParkDto.Id) &&
                        (string.IsNullOrEmpty(ParkName) ? 1 == 1 : m.ParkName.ToUpper().StartsWith(ParkName.ToUpper())) &&
                        (SpacesToRent == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)m.SpacesToRent).StartsWith(SqlFunctions.StringConvert((double)SpacesToRent))) &&
                        (SpacesToOwn == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)m.SpacesToOwn).StartsWith(SqlFunctions.StringConvert((double)SpacesToOwn))) &&
                        (string.IsNullOrEmpty(PhysicalAddress) ? 1 == 1 : m.PhysicalAddress.ToUpper().StartsWith(PhysicalAddress.ToUpper())) &&
                        (string.IsNullOrEmpty(State) ? 1 == 1 : m.PhysicalState.Abbr.ToUpper().StartsWith(State.ToUpper())) &&
                        (ParkDto.PhysicalZip == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)m.PhysicalZip).StartsWith(SqlFunctions.StringConvert((double)ParkDto.PhysicalZip)))                
                    ).ToList();

                    searchParam.TotalRecordCount = items.Count();
                }
                result = items.Select(x =>
                    new ParkDto()
                    {
                        Id = x.Id,
                        IsActive = x.IsActive,
                        ParkName = x.ParkName,
                        PhysicalAddress = x.PhysicalAddress,
                        PhysicalStateId = x.PhysicalStateId,
                        PhysicalZip = x.PhysicalZip,
                        SpacesToOwn = x.SpacesToOwn,
                        SpacesToRent = x.SpacesToRent,
                        State = (x.PhysicalStateId != null || x.PhysicalStateId != 0) ? x.PhysicalState.Abbr : ""
                    }
                ).ToList();
                
            }
            
            searchParam.SearchedCount = (!searchParam.IsFilterValue ? searchParam.TotalRecordCount : result.Count);

            return result;
        }

        #region Generic Search
        //private SearchQuery<Park> CreateFilter(SearchParameter searchParam)
        //{
        //    var query = new SearchQuery<Park>();
        //    if (searchParam != null)
        //    {
        //        var isFilterValue = searchParam.SearchColumnValue.Any(e => !string.IsNullOrWhiteSpace(e));

        //        searchParam.IsFilterValue = isFilterValue;

        //        if ((searchParam.SearchColumn != null && searchParam.SearchColumn.Count > 0) &&
        //            searchParam.SearchColumn.Count == searchParam.SearchColumnValue.Count - 1 && isFilterValue) // minus -1 means, skipping action column from search list
        //        {
        //            var filterValueProp = new Dictionary<string, string>();
        //            for (int idx = 0; idx < searchParam.SearchColumnValue.Count; idx++)
        //            {
        //                if (!string.IsNullOrWhiteSpace(searchParam.SearchColumnValue[idx]))
        //                {
        //                    if (searchParam.SearchColumn[idx] == "Id") query.AddFilter(p => p.Id == Convert.ToInt32(searchParam.SearchColumnValue[idx]));
        //                    else if (searchParam.SearchColumn[idx] == "ParkName") query.AddFilter(p => p.ParkName == searchParam.SearchColumnValue[idx]);
        //                    else if (searchParam.SearchColumn[idx] == "SpacesToRent") query.AddFilter(p => p.SpacesToRent == Convert.ToInt32(searchParam.SearchColumnValue[idx]));
        //                    else if (searchParam.SearchColumn[idx] == "SpacesToOwn") query.AddFilter(p => p.SpacesToOwn == Convert.ToInt32(searchParam.SearchColumnValue[idx]));
        //                    else if (searchParam.SearchColumn[idx] == "PhysicalAddress") query.AddFilter(p => p.PhysicalAddress == searchParam.SearchColumnValue[idx]);
        //                    else if (searchParam.SearchColumn[idx] == "State") query.AddFilter(p => p.PhysicalState.Name == searchParam.SearchColumnValue[idx]);
        //                    else if (searchParam.SearchColumn[idx] == "PhysicalZip") query.AddFilter(p => p.PhysicalZip == Convert.ToInt32(searchParam.SearchColumnValue[idx]));                                                     

        //                }
        //            }
        //        }

        //    }
        //    return query;
        //}
        //public List<ParkDto> SearchPark(SearchParameter searchParam)
        //{
        //    var productRepository = new Repository<Park>(new mhappraisalContext());
        //    var query = CreateFilter(searchParam);
        //    var result = productRepository.Search(query);
        //    List<ParkDto> data = new List<ParkDto>();
        //    foreach (var x in result.Entities)
        //    {
        //        ParkDto objParkDto =  new ParkDto()
        //        {
        //            Id = x.Id,
        //            IsActive = x.IsActive,
        //            ParkName = x.ParkName,
        //            PhysicalAddress = x.PhysicalAddress,
        //            PhysicalStateId = x.PhysicalStateId,
        //            PhysicalZip = x.PhysicalZip,
        //            SpacesToOwn = x.SpacesToOwn,
        //            SpacesToRent = x.SpacesToRent,
        //            State = (x.PhysicalStateId != null || x.PhysicalStateId != 0) ? x.PhysicalState.Abbr : ""
        //        };
        //        data.Add(objParkDto);
        //    }
            
        //    return data;
        //}
        #endregion
        public List<ParkSiteDto> GetParkSites()
        {
            _context.Configuration.ProxyCreationEnabled = true;
            _context.Configuration.LazyLoadingEnabled = true;

            var parkSitesList = _context.ParkSites.Where(x=>x.IsActive == true).ToList();
            var rtnItems = parkSitesList.Select(x => new ParkSiteDto()
            {
                Id = x.Id,
                ParkName = x.Park.ParkName,
                PhysicalCity = x.PhysicalCity,
                PhysicalState = x.State.Name,
                PhysicalZip = x.PhysicalZip.Value,
                TenantFirstName = x.TenantFirstName,
                TenantLastName = x.TenantLastName,
                Premium = (x.Quote!=null ? Convert.ToInt32(x.Quote.Premium): 0),
                SiteNumber =Convert.ToInt32(x.SiteNumber)
                    
            }).ToList();

            return rtnItems;
        }
        private ParkSiteDto GetParkSiteObject(SearchParameter searchParam)
        {
            ParkSiteDto parkObj = new ParkSiteDto();
            if (searchParam != null)
            {
                var isFilterValue = searchParam.SearchColumnValue.Any(e => !string.IsNullOrWhiteSpace(e));

                searchParam.IsFilterValue = isFilterValue;

                if ((searchParam.SearchColumn != null && searchParam.SearchColumn.Count > 0) &&
                    searchParam.SearchColumn.Count == searchParam.SearchColumnValue.Count && isFilterValue) // minus -1 means, skipping action column from search list
                {
                    var filterValueProp = new Dictionary<string, string>();
                    for (int idx = 0; idx < searchParam.SearchColumnValue.Count; idx++)
                    {
                        if (!string.IsNullOrWhiteSpace(searchParam.SearchColumnValue[idx]))
                        {
                            if (searchParam.SearchColumn[idx] == "Id") parkObj.Id = Convert.ToInt32(searchParam.SearchColumnValue[idx]);
                            else if (searchParam.SearchColumn[idx] == "ParkName") parkObj.ParkName = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "SiteNumber") parkObj.SiteNumber = Convert.ToInt32(searchParam.SearchColumnValue[idx]);
                            else if (searchParam.SearchColumn[idx] == "TenantFirstName") parkObj.TenantFirstName = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "TenantLastName") parkObj.TenantLastName = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "Premium") parkObj.Premium = Convert.ToDecimal(searchParam.SearchColumnValue[idx]);
                            else if (searchParam.SearchColumn[idx] == "PhysicalCity") parkObj.PhysicalCity = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "PhysicalState") parkObj.PhysicalState = searchParam.SearchColumnValue[idx];
                            else if (searchParam.SearchColumn[idx] == "PhysicalZip") parkObj.PhysicalZip = Convert.ToInt32(searchParam.SearchColumnValue[idx]);                                                     
                        }
                    }
                }

            }
            return parkObj;
        }
        public List<ParkSiteDto> GetParkSites(SearchParameter searchParam)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            List<ParkSiteDto> result = null;
            if (searchParam != null)
            {
                List<ParkSite> items = null;
                ParkSiteDto ParkDto = GetParkSiteObject(searchParam);
                Int32 SiteNumber = Convert.ToInt32(ParkDto.SiteNumber);
                decimal Premium = Convert.ToDecimal(ParkDto.Premium);                              
                string PhysicalZip = Convert.ToString(ParkDto.PhysicalZip);

                if (!searchParam.IsFilterValue)
                {
                    searchParam.TotalRecordCount = _context.ParkSites.Count();
                    items = _context.ParkSites.Include("State")
                        .Include("Park")
                        .Include("Quote").Where(p=>p.IsActive==true).
                    OrderBy(x => x.Id)
                    .Skip(searchParam.StartIndex).Take((searchParam.PageSize > 0 ? searchParam.PageSize : searchParam.TotalRecordCount)).
                    ToList();
                }
                else
                {
                    items = _context.ParkSites.Include("State")
                        .Include("Park")
                        .Include("Quote")
                        .Where(m => m.IsActive==true &&
                        (ParkDto.Id == 0 ? 1 == 1 : m.Id == ParkDto.Id) &&
                        (string.IsNullOrEmpty(ParkDto.ParkName) ? 1 == 1 : m.Park.ParkName.ToUpper().StartsWith(ParkDto.ParkName.ToUpper())) &&
                        (SiteNumber == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)m.SiteNumber).StartsWith(SqlFunctions.StringConvert((double)SiteNumber))) &&
                        (Premium == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)m.Quote.Premium).StartsWith(SqlFunctions.StringConvert((double)Premium))) &&
                        (string.IsNullOrEmpty(ParkDto.PhysicalCity) ? 1 == 1 : m.PhysicalCity.ToUpper().StartsWith(ParkDto.PhysicalCity.ToUpper())) &&
                        (string.IsNullOrEmpty(ParkDto.PhysicalState) ? 1 == 1 : m.State.Abbr.ToUpper().StartsWith(ParkDto.PhysicalState.ToUpper())) &&
                        (ParkDto.PhysicalZip == 0 ? 1 == 1 : SqlFunctions.StringConvert((double)m.PhysicalZip).StartsWith(SqlFunctions.StringConvert((double)ParkDto.PhysicalZip)))
                    ).ToList();

                    searchParam.TotalRecordCount = items.Count();
                }
                result = items.Select(x =>
                    new ParkSiteDto()
                    {
                        Id = x.Id,
                        ParkName = (x.Park!=null? x.Park.ParkName:string.Empty),
                        PhysicalCity = x.PhysicalCity,
                        PhysicalState = (x.State!=null?x.State.Name:string.Empty),
                        PhysicalZip = x.PhysicalZip,
                        TenantFirstName = x.TenantFirstName,
                        TenantLastName = x.TenantLastName,
                        Premium = (x.Quote != null ? Convert.ToInt32(x.Quote.Premium) : 0),
                        SiteNumber = Convert.ToInt32(x.SiteNumber)
                    }
                ).ToList();

            }

            searchParam.SearchedCount = (!searchParam.IsFilterValue ? searchParam.TotalRecordCount : result.Count);

            return result;
        }

        public Park GetParkById(int id)
        {
            return _context.Parks.Where(x => x.Id == id && x.IsActive == true).SingleOrDefault();
        }
        public ParkSite GetParkSiteById(int id)
        {
            return _context.ParkSites.Include("Park").
                Include("Quote").Include("Quote.Company").Where(x => x.Id == id && x.IsActive == true).SingleOrDefault();
        }
        /// <summary>
        /// Find Park by zip
        /// </summary>
        /// <param name="zip">Pass zip of the location.</param>
        /// <returns>Return list of Park based on Zip.</returns>
        public List<Park> FindParkByZip(int zip)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            var parks = _context.Parks.Where(p => p.PhysicalZip == zip).ToList(); //TODO: Filter should  be based on PhysicalZip
            if (parks != null)
                parks = parks.Where(p => p.IsActive == true).ToList();
            return ((parks != null && parks.Count > 0) ? parks : null);
        }

        public void SavePark(Park parkObj, bool toDelete = false)
        {
            if (parkObj.Id != 0)
            {
                var existingObj = _context.Parks.Where(x => x.Id == parkObj.Id).SingleOrDefault();
                if (existingObj != null)
                {
                    if (toDelete)
                        existingObj.IsActive = false;
                    else
                    {
                        existingObj.ParkName = parkObj.ParkName;
                        existingObj.PhysicalAddress = parkObj.PhysicalAddress;
                        existingObj.PhysicalAddress2 = parkObj.PhysicalAddress2;
                        existingObj.PhysicalCity = parkObj.PhysicalCity;
                        existingObj.PhysicalZip = parkObj.PhysicalZip;
                        existingObj.PhysicalStateId = parkObj.PhysicalStateId;
                        existingObj.PhysicalCounty = parkObj.PhysicalCounty;

                        existingObj.OfficePhone = parkObj.OfficePhone;
                        existingObj.OfficeFax = parkObj.OfficeFax;
                        existingObj.OfficeMail = parkObj.OfficeMail;
                        existingObj.Website = parkObj.Website;

                        existingObj.SpacesToRent = parkObj.SpacesToRent;
                        existingObj.SpacesToOwn = parkObj.SpacesToOwn;
                        existingObj.ContactName1 = parkObj.ContactName1;
                        existingObj.ContactName2 = parkObj.ContactName2;
                        existingObj.Position = parkObj.Position;

                        //Mailing
                        existingObj.MailingName = parkObj.MailingName;
                        existingObj.MailingAddress = parkObj.MailingAddress;
                        existingObj.MailingAddress2 = parkObj.MailingAddress2;
                        existingObj.MailingCity = parkObj.MailingCity;
                        existingObj.MailingZip = parkObj.MailingZip;
                        existingObj.MailingStateId = parkObj.MailingStateId;


                        //Owner
                        existingObj.OwnerAddress = parkObj.OwnerAddress;
                        existingObj.OwnerAddress2 = parkObj.OwnerAddress2;
                        existingObj.OwnerCity = parkObj.OwnerCity;
                        existingObj.OwnerZip = parkObj.OwnerZip;
                        existingObj.OwnerStateId = parkObj.OwnerStateId;
                        existingObj.OwnerPhone = parkObj.OwnerPhone;


                        existingObj.IsActive = parkObj.IsActive;
                    }
                    _context.Entry(existingObj).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    sendNotificationForPark(parkObj, true);
                }
            }
            else
            {
                parkObj.CreatedBy = "admin";
                parkObj.CreatedDate = DateTime.Now;
                parkObj.IsActive = true;
                _context.Parks.Add(parkObj);
                _context.SaveChanges();
                sendNotificationForPark(parkObj, false);
            }
        }

        public void SaveParkSite(ParkSite parkSiteObj, bool toDelete = false)
        {
            _context.Configuration.ValidateOnSaveEnabled = false;
            if (parkSiteObj.Id != 0)
            {
                var existingObj = _context.ParkSites.Include("Quote").Where(x => x.Id == parkSiteObj.Id).SingleOrDefault();
                if (existingObj != null)
                {
                    if (toDelete)
                        existingObj.IsActive = false;
                    else
                    {
                        existingObj.ParkId = parkSiteObj.ParkId;
                        existingObj.TenantFirstName = parkSiteObj.TenantFirstName;
                        existingObj.TenantLastName = parkSiteObj.TenantLastName;
                        existingObj.SiteNumber = parkSiteObj.SiteNumber;
                        existingObj.PhysicalAddress1 = parkSiteObj.PhysicalAddress1;
                        existingObj.PhysicalAddress2 = parkSiteObj.PhysicalAddress2;
                        existingObj.PhysicalCity = parkSiteObj.PhysicalCity;
                        existingObj.PhysicalStateId = parkSiteObj.PhysicalStateId;
                        existingObj.PhysicalZip = parkSiteObj.PhysicalZip;
                        existingObj.Quote.IsParkSitePolicy = true;
                        existingObj.Quote.EffectiveDate = parkSiteObj.Quote.EffectiveDate;
                        existingObj.Quote.ExpiryDate = parkSiteObj.Quote.ExpiryDate;
                        existingObj.Quote.CompanyId = parkSiteObj.Quote.CompanyId;
                        existingObj.Quote.Liability = parkSiteObj.Quote.Liability;
                        existingObj.Quote.Premium = parkSiteObj.Quote.Premium;                        
                    }
                    _context.Entry(existingObj).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();                   
                }
            }
            else
            {                
                parkSiteObj.CreatedDate = DateTime.Now;
                parkSiteObj.IsActive = true;
                _context.ParkSites.Add(parkSiteObj);
                _context.SaveChanges();
                
            }
        }

        /// <summary>
        /// Bulk save multiple Parks while importing from external.
        /// </summary>
        /// <param name="importParks">Pass List of parks for saving.</param>
        public void SavePark(List<Park> importParks)
        {
            importParks.ForEach(item =>
            {
                item.CreatedBy = "admin";
                item.CreatedDate = DateTime.Now;
                //item.IsActive = true;
            });
            _context.Parks.AddRange(importParks);
            _context.SaveChanges();
            importParks.ForEach(x => sendNotificationForPark(x, false));
        }

        public void sendNotificationForPark(Park parkObj, bool isEdit)
        {            
            List<string> emailNotification = null;
            string body ="";
            if (!isEdit)
            {
                emailNotification = _context.ParkNotifies.Where(x => x.Zip == parkObj.PhysicalZip.ToString() && x.IsNotified == false).Select(y => y.Email).ToList();
                body = "New Park for your Zip is Added <br /> Park Name: " + parkObj.ParkName + " <br / > Address: " + parkObj.PhysicalAddress + " " + parkObj.PhysicalAddress2 + " <br /> Park City: " + parkObj.PhysicalCity + "<br /> Park County: " + parkObj.PhysicalCounty + "<br /> Park State: " + parkObj.PhysicalState.Name + "<br /> Park Zip: " + parkObj.PhysicalZip;
            }
            else
            {
                _rentalcontext = new mhRentalContext();
                emailNotification = _rentalcontext.Customers.Where(x => x.Zip == parkObj.PhysicalZip.ToString() && x.IsActive == true).Select(y => y.Email).ToList();
                 body = "Your park information has been edited. Here are the details: <br /> Park Name: " + parkObj.ParkName + " <br / > Address: " + parkObj.PhysicalAddress + " " + parkObj.PhysicalAddress2 + " <br /> Park City: " + parkObj.PhysicalCity + "<br /> Park County: " + parkObj.PhysicalCounty + "<br /> Park State: " + parkObj.PhysicalState.Name + "<br /> Park Zip: " + parkObj.PhysicalZip;
            }
            _genralServiceFacade.sendMail("info@mobilehome.insure", "info@mobilehome.insure", "", body, emailNotification);
              
        }

        public bool OnOrOffPark(int id, bool isOff = false)
        {
            bool result = false;
            if (id > 0)
            {
                var existingObj = _context.Parks.Where(x => x.Id == id).SingleOrDefault();
                if (existingObj != null)
                {
                    existingObj.IsActive = isOff;
                    _context.Entry(existingObj).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    result = true;
                }
            }

            return result;
        }

        public List<ParkDto> GetListParks(string parkName, int stateId, string zipCode)
        {
            
            _context.Configuration.ProxyCreationEnabled = false;
            Int32 tZipCode = 0;
            Int32.TryParse(zipCode,out tZipCode);
            var items = _context.Parks.Include("Customers").
                Include("PhysicalState").
                Where(p => (string.IsNullOrEmpty(parkName) ? 1 == 1 : p.ParkName == parkName) &&
                                             (stateId == 0 ? 1 == 1 : p.PhysicalStateId == stateId) &&
                                             (string.IsNullOrEmpty(zipCode) ? 1 == 1 : p.PhysicalZip == tZipCode)).ToList();

            //var state = _context.States.ToList();

            var rtnItems = items.Select(x => new ParkDto()
            {
                Id = x.Id,
                IsActive = x.IsActive,
                ParkName = x.ParkName,
                PhysicalAddress = x.PhysicalAddress,
                PhysicalStateId = x.PhysicalStateId,
                PhysicalZip = x.PhysicalZip,
                PhysicalCity = x.PhysicalCity,
                TotalOwnRentals = (x.Customers != null ? x.Customers.Count() : 0),
                SpacesToOwn = x.SpacesToOwn,
                SpacesToRent = x.SpacesToRent,
                State = (x.PhysicalState!=null?x.PhysicalState.Name:string.Empty)
                //State = (x.PhysicalStateId != null || x.PhysicalStateId != 0) ? state.Where(y => y.Id == x.PhysicalStateId).SingleOrDefault().Name : ""
            }).ToList();
            return rtnItems;
        }
        #endregion

        #region state
        public List<ParkNotify> GetNotifies()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.ParkNotifies.AsNoTracking().Where(x => x.IsNotified != true).OrderBy(x => x.CreatedOn).ToList();
        }

        public ParkNotify GetNotifyById(int id)
        {
            return _context.ParkNotifies.Where(x => x.Id == id).SingleOrDefault();
        }

        public void saveNotify(ParkNotify notifyObj, bool toDelete = false)
        {
            if (notifyObj.Id != 0)
            {
                var existingObj = _context.ParkNotifies.Where(x => x.Id == notifyObj.Id).SingleOrDefault();
                if (existingObj != null)
                {
                    if (toDelete)
                        existingObj.IsNotified = true;
                    else
                    {
                        existingObj.Name = notifyObj.Name;
                        existingObj.Email = notifyObj.Email;
                        existingObj.Zip = notifyObj.Zip;
                    }
                    _context.Entry(existingObj).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            else
            {
                notifyObj.CreatedOn = DateTime.Now;
                notifyObj.IsNotified = false;
                _context.ParkNotifies.Add(notifyObj);
                _context.SaveChanges();
            }
        }

        #endregion

        
        #region Common methods for Date operation
        public static string GetDateFormatAsString(DateTime date)
        {
            return string.Format("{0}-{1}-{2}", date.Month, date.Day, date.Year);
        }

        public static DateTime GetStringAsDateFormat(string date)
        {
            DateTime format = DateTime.MinValue;
            var splitDate = date.Split(new char[] { '-', '/', '.' });

            if (splitDate != null && splitDate.Length == 3)
            {
                if (Convert.ToInt32(splitDate[0]) > 12) //for 23-09-2015
                    format = new DateTime(Convert.ToInt32(splitDate[2]), Convert.ToInt32(splitDate[1]), Convert.ToInt32(splitDate[0]));
                else if (Convert.ToInt32(splitDate[1]) > 12) //for 09-23-2015
                    format = new DateTime(Convert.ToInt32(splitDate[2]), Convert.ToInt32(splitDate[0]), Convert.ToInt32(splitDate[1]));
                else //for 09-09-2015 (MM-dd-yyyy)
                    format = new DateTime(Convert.ToInt32(splitDate[2]), Convert.ToInt32(splitDate[0]), Convert.ToInt32(splitDate[1]));
            }

            return format;
        }
        #endregion
    }
}
