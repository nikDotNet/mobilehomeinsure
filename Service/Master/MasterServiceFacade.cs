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


        public void saveState(State objState)
        {
            _context.States.Add(objState);
            _context.SaveChanges();
        }

        public void updateState(State objState)
        {
            var obj =  _context.States.Where(x => x.Id == objState.Id).FirstOrDefault();
            if (obj != null)
            {
                obj = objState;
                _context.SaveChanges();
            }
        }


        public void saveManufacturer(Manufacturer objManufacturer)
        {
            _context.Manufacturers.Add(objManufacturer);
            _context.SaveChanges();
        }

        public void updateManufacturer(Manufacturer objManufacturer)
        {
            var obj = _context.Manufacturers.Where(x => x.Id == objManufacturer.Id).FirstOrDefault();
            if (obj != null)
            {
                obj = objManufacturer;
                _context.SaveChanges();
            }
        }




    }
}
