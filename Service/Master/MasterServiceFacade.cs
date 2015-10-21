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

        public MasterServiceFacade()
        {
            _context = new mhappraisalContext();
            _rentalcontext = new mhRentalContext();
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
            return _context.Parks.AsNoTracking().ToList();
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
                State = (x.PhysicalStateId != null || x.PhysicalStateId != 0) ? state.Where(y => y.Id == x.PhysicalStateId).SingleOrDefault().Name : ""
            }).ToList();
            return rtnItems;
        }

        public Park GetParkById(int id)
        {
            return _context.Parks.Where(x => x.Id == id && x.IsActive == true).SingleOrDefault();
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
                        //existingObj.StateId = parkObj.StateId;
                        //existingObj.Name = parkObj.Name;
                        //existingObj.Address = parkObj.Address;
                        //existingObj.City = parkObj.City;
                        //existingObj.Zip = parkObj.Zip;
                        //existingObj.Zip4 = parkObj.Zip4;
                        //existingObj.County = parkObj.County;
                        //existingObj.Phone = parkObj.Phone;
                        //existingObj.Spaces = parkObj.Spaces;
                        //existingObj.ContactName = parkObj.ContactName;
                        //existingObj.Position = parkObj.Position;
                        //existingObj.IsActive = parkObj.IsActive;


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
                }
            }
            else
            {
                parkObj.CreatedBy = "admin";
                parkObj.CreatedDate = DateTime.Now;
                parkObj.IsActive = true;
                _context.Parks.Add(parkObj);
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
        #endregion

        #region state
        public List<ParkNotify> GetNotifies()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.ParkNotifies.AsNoTracking().OrderBy(x => x.CreatedOn).ToList();
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

        public List<OrderDto> GetListOrder()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            var items = _rentalcontext.Payments.Where(x => x.TransactionId != null).ToList();
            var state = _context.States.ToList();
            var rtnItems = items.Select(x => new OrderDto()
            {
                OrderId = x.Id,

                ApprovalCode = x.ApprovalCode,
                ApprovalMessage = x.ApprovalMessage,
                CreatedBy = x.CreatedBy,
                CreationDate = x.CreationDate.Value,
                ErrorMessage = x.ErrorMessage,
                ResponseCode = x.ResponseCode,
                TransactionId = x.TransactionId,

                RenterId = x.RentalQuoteId.Value,
                CompanyId = x.Quote != null ? x.Quote.CompanyId.Value : 0,
                CompanyName = x.Quote != null && x.Quote.Company != null ? x.Quote.Company.Name : "",
                ProposalNumber = x.Quote != null ? x.Quote.ProposalNumber : "",

                CustomerId = x.CustomerId.Value,
                CustomerName = x.Customer != null ? x.Customer.FirstName + " " + x.Customer.LastName : ""

            }).ToList();
            return rtnItems;
        }

        public List<Customer> GetListCustomers(string zipCode, string lastName)
        {
            List<Customer> items = null;
            //.Include("States")
            _rentalcontext.Configuration.ProxyCreationEnabled = false;
            if (string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(zipCode))
                items = _rentalcontext.Customers.ToList();

            if (!string.IsNullOrWhiteSpace(zipCode) && string.IsNullOrWhiteSpace(lastName))
                items = _rentalcontext.Customers.Where(x => x.Zip == zipCode).ToList();

            if (!string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(zipCode))
                items = _rentalcontext.Customers.Where(x => x.LastName == lastName).ToList();

            if (!string.IsNullOrWhiteSpace(lastName) && !string.IsNullOrWhiteSpace(zipCode))
                items = _rentalcontext.Customers.Where(x => x.Zip == zipCode && x.LastName == lastName).ToList();

            return (items != null && items.Count > 0) ? items : null;
        }

        public List<OrderDto> GetListPremiums(int stateId, string zipCode, DateTime startDate, DateTime endDate)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            var items = _rentalcontext.Payments.Where(x => x.TransactionId != null).ToList();
            var state = _context.States.ToList();
            var rtnItems = items.Select(x => new OrderDto()
            {
                OrderId = x.Id,

                ApprovalCode = x.ApprovalCode,
                ApprovalMessage = x.ApprovalMessage,
                CreatedBy = x.CreatedBy,
                CreationDate = x.CreationDate.Value,
                ErrorMessage = x.ErrorMessage,
                ResponseCode = x.ResponseCode,
                TransactionId = x.TransactionId,

                RenterId = x.RentalQuoteId.Value,
                CompanyId = x.Quote != null ? x.Quote.CompanyId.Value : 0,
                CompanyName = x.Quote != null && x.Quote.Company != null ? x.Quote.Company.Name : "",
                ProposalNumber = x.Quote != null ? x.Quote.ProposalNumber : "",

                CustomerId = x.CustomerId.Value,
                CustomerName = x.Customer != null ? x.Customer.FirstName + " " + x.Customer.LastName : ""

            }).ToList();
            return rtnItems;
        }
        #endregion

    }
}
