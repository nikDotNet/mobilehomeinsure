using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileHome.Insure.DAL.EF;
using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Rental;
using MobileHome.Insure.Model.DTO;

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
            _rentalcontext = new mhRentalContext();
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
                Premium = x.Quote.Premium.Value,
                SiteNumber = x.SiteNumber.Value
            }).ToList();

            return rtnItems;
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
            if (parkSiteObj.Id != 0)
            {
                var existingObj = _context.ParkSites.Include("Quote").Where(x => x.Id == parkSiteObj.Id).SingleOrDefault();
                if (existingObj != null)
                {
                    if (toDelete)
                        existingObj.IsActive = false;
                    else
                    {
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

        #region Reporting

        public List<OrderDto> GetListOrder(string startDate, string endDate)
        {
            List<Model.Payment> items = null;

            _rentalcontext.Configuration.ProxyCreationEnabled = false;
            if (string.IsNullOrWhiteSpace(startDate) && string.IsNullOrWhiteSpace(endDate))
                items = _rentalcontext.Payments.Include("Customer").
                    Include("Quote").
                    Include("Quote.Company").
                    Where(x => x.TransactionId != null).ToList();
            else if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                var startDt = GetStringAsDateFormat(startDate);
                var endDt = GetStringAsDateFormat(endDate).AddDays(1);
                items = _rentalcontext.Payments.Include("Customer").
                    Include("Quote").
                    Include("Quote.Company").
                    Where(x => x.TransactionId != null && (x.CreationDate >= startDt && x.CreationDate <= endDt)).ToList();
            }


            var rtnItems = items.Select(x => new OrderDto()
            {
                OrderId = x.Id,

                ApprovalCode = x.ApprovalCode,
                ApprovalMessage = x.ApprovalMessage,
                CreatedBy = x.CreatedBy,
                //CreationDate = x.CreationDate.HasValue ? x.CreationDate.Value.Date : DateTime.MinValue,
                CreationDateStr = x.CreationDate.HasValue ? GetDateFormatAsString(x.CreationDate.Value) : string.Empty,
                ErrorMessage = (x.ErrorMessage!=null ? x.ErrorMessage: string.Empty),
                ResponseCode = x.ResponseCode,
                TransactionId = x.TransactionId,

                RenterId = x.RentalQuoteId.Value,
                CompanyId = x.Quote != null && x.Quote.Company != null ? x.Quote.CompanyId.Value : 0,
                CompanyName = x.Quote != null && x.Quote.Company != null ? x.Quote.Company.Name : string.Empty,
                ProposalNumber = x.Quote != null ? x.Quote.ProposalNumber : string.Empty,

                CustomerId = x.CustomerId.Value,
                CustomerName = x.Customer != null ? x.Customer.FirstName + " " + x.Customer.LastName : string.Empty

            }).ToList();
            return rtnItems;
        }

        public List<OrderDto> GetListPremiums(int stateId, string zipCode, string startDate, string endDate)
        {
            List<Model.Payment> items = null;

            _rentalcontext.Configuration.ProxyCreationEnabled = false;
            if (stateId == 0 & string.IsNullOrWhiteSpace(zipCode) && string.IsNullOrWhiteSpace(startDate) && string.IsNullOrWhiteSpace(endDate))
                items = _rentalcontext.Payments.Where(x => x.TransactionId != null).ToList();

            if (stateId > 0 & !string.IsNullOrWhiteSpace(zipCode) && !string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                var startDt = GetStringAsDateFormat(startDate);
                var endDt = GetStringAsDateFormat(endDate).AddDays(1);

                items = _rentalcontext.Payments
                                      .Where(x => x.TransactionId != null &&
                                            (x.Customer != null && x.Customer.StateId != null && x.Customer.StateId == stateId) &&
                                            (x.Customer != null && x.Customer.Zip == zipCode) &&
                                            (x.CreationDate != null && x.CreationDate.Value >= startDt && x.CreationDate.Value <= endDt)).ToList();

            }
            else if (stateId > 0 || !string.IsNullOrWhiteSpace(zipCode) || (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate)))
            {
                DateTime startDt = DateTime.MinValue, endDt = DateTime.MinValue;
                if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
                {
                    startDt = GetStringAsDateFormat(startDate);
                    endDt = GetStringAsDateFormat(endDate).AddDays(1);
                }
                else
                    startDate = endDate = null;

                items = _rentalcontext.Payments
                                      .Where(x => x.TransactionId != null &&
                                            (stateId == 0 || (x.Customer != null && x.Customer.StateId != null && x.Customer.State.Id == stateId)) &&
                                            ((zipCode == null || zipCode.Trim() == string.Empty) || (x.Customer != null && x.Customer.Zip == zipCode)) &&
                                            ((startDate == null || (x.CreationDate.Value >= startDt)) &&
                                            (endDate == null || (x.CreationDate.Value <= endDt)))).ToList();
            }

            List<OrderDto> rtnItems = null;
            if (items != null && items.Count > 0)
            {
                rtnItems = new List<OrderDto>();
                items.ForEach((rec) =>
                {
                    var totPremium = _rentalcontext.Payments.Where(p => rec.CustomerId != null && p.CustomerId == rec.CustomerId).Sum(tp => tp.Amount);
                    var customer = _rentalcontext.Customers.SingleOrDefault(c => c.Id == rec.CustomerId);
                    var quote = _rentalcontext.Quotes.SingleOrDefault(q => q.Id == rec.RentalQuoteId.Value);
                    rtnItems.Add(new OrderDto()
                               {
                                   OrderId = rec.Id,

                                   TotalPremium = totPremium,
                                   ApprovalCode = rec.ApprovalCode,
                                   ApprovalMessage = rec.ApprovalMessage,
                                   CreatedBy = rec.CreatedBy,
                                   //CreationDate = rec.CreationDate != null ? rec.CreationDate.Value.Date : DateTime.MinValue,
                                   CreationDateStr = rec.CreationDate != null ? GetDateFormatAsString(rec.CreationDate.Value) : string.Empty,
                                   ErrorMessage = rec.ErrorMessage,
                                   ResponseCode = rec.ResponseCode,
                                   TransactionId = rec.TransactionId,

                                   RenterId = rec.RentalQuoteId.Value,
                                   CompanyId = quote != null && quote.Company != null ? quote.CompanyId.Value : 0,
                                   CompanyName = quote != null && quote.Company != null ? quote.Company.Name : "",
                                   ProposalNumber = quote != null ? quote.ProposalNumber : "",

                                   CustomerId = rec.CustomerId.Value,
                                   CustomerName = customer != null ? customer.FirstName + " " + customer.LastName : "",
                                   ZipCode = customer != null ? customer.Zip : "",
                                   Phone = customer != null ? customer.Phone : "",
                                   Email = customer != null ? customer.Email : "",

                               });
                });
            }

            return rtnItems;
        }


        //public List<Customer> GetListCustomers(string zipCode, string lastName)
        //{
        //    List<Customer> items = null;
        //    //.Include("States")
        //    _rentalcontext.Configuration.ProxyCreationEnabled = false;
        //    if (string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(zipCode))
        //        items = _rentalcontext.Customers.ToList();

        //    if (!string.IsNullOrWhiteSpace(zipCode) && string.IsNullOrWhiteSpace(lastName))
        //        items = _rentalcontext.Customers.Where(x => x.Zip == zipCode).ToList();

        //    if (!string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(zipCode))
        //        items = _rentalcontext.Customers.Where(x => x.LastName == lastName).ToList();

        //    if (!string.IsNullOrWhiteSpace(lastName) && !string.IsNullOrWhiteSpace(zipCode))
        //        items = _rentalcontext.Customers.Where(x => x.Zip == zipCode && x.LastName.StartsWith(lastName)).ToList();

        //    return (items != null && items.Count > 0) ? items : null;
        //}

        //public List<ParkDto> GetListParks(string parkName, int stateId, string zipCode)
        //{
        //    List<Park> items = null;

        //    _context.Configuration.ProxyCreationEnabled = false;
        //    if (string.IsNullOrWhiteSpace(parkName) && stateId == 0 && string.IsNullOrWhiteSpace(zipCode))
        //        items = _context.Parks.AsNoTracking().ToList();

        //    if (!string.IsNullOrWhiteSpace(parkName) && stateId > 0 && !string.IsNullOrWhiteSpace(zipCode))
        //    {
        //        var zipInt = Convert.ToInt32(zipCode);
        //        items = _context.Parks.AsNoTracking().Where(p => p.ParkName.StartsWith(parkName) && p.PhysicalZip == zipInt).ToList();
        //    }

        //    if (!string.IsNullOrWhiteSpace(parkName) || stateId > 0 | !string.IsNullOrWhiteSpace(zipCode))
        //    {
        //        if (string.IsNullOrWhiteSpace(parkName))
        //            parkName = null;

        //        int zipInt = 0;
        //        if (string.IsNullOrWhiteSpace(zipCode))
        //            zipCode = null;
        //        else
        //            zipInt = !string.IsNullOrWhiteSpace(zipCode) ? Convert.ToInt32(zipCode) : 0;


        //        items = _context.Parks.AsNoTracking().Where(p =>
        //                                                    (parkName == null || p.ParkName.StartsWith(parkName)) &&
        //                                                    (stateId == 0 || (p.PhysicalStateId != null && p.PhysicalStateId.Value == stateId)) &&
        //                                                    (zipCode == null || (p.PhysicalZip == zipInt))).ToList();
        //    }

        //    var rtnItems = items.Select(x => new ParkDto()
        //    {
        //        Id = x.Id,
        //        ParkName = x.ParkName,
        //        PhysicalAddress = x.PhysicalAddress,
        //        PhysicalZip = x.PhysicalZip,
        //        PhysicalCity = x.PhysicalCity,
        //        TotalOwnRentals = _rentalcontext.Customers.Where(c => c.ParkId == x.Id).Count()
        //    }).ToList();
        //    return rtnItems;
        //}
        #endregion


        #region Common methods for Date operation
        private string GetDateFormatAsString(DateTime date)
        {
            return string.Format("{0}-{1}-{2}", date.Month, date.Day, date.Year);
        }

        private DateTime GetStringAsDateFormat(string date)
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
