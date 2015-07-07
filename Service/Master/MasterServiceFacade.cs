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
             return _context.States.Where(x => x.isActive == true).ToList();
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

    }
}
