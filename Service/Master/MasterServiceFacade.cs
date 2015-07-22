using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileHome.Insure.DAL.EF;
using MobileHome.Insure.Model;

namespace MobileHome.Insure.Service.Master
{
    public class MasterServiceFacade : IMasterServiceFacade
    {
        private readonly mhappraisalContext _context;

        public MasterServiceFacade()
        {
            _context = new mhappraisalContext();
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
            return _context.States.AsNoTracking().Where(x => x.isActive == true).ToList();
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
            var parks = _context.Parks.Where(p => p.Zip == zip).ToList();
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
                        existingObj.StateId = parkObj.StateId;
                        existingObj.Name = parkObj.Name;
                        existingObj.Address = parkObj.Address;
                        existingObj.City = parkObj.City;
                        existingObj.Zip = parkObj.Zip;
                        existingObj.Zip4 = parkObj.Zip4;
                        existingObj.County = parkObj.County;
                        existingObj.Phone = parkObj.Phone;
                        existingObj.Spaces = parkObj.Spaces;
                        existingObj.ContactName = parkObj.ContactName;
                        existingObj.Position = parkObj.Position;
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
        #endregion
    }
}
