using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;

namespace Service.Appraisal
{
    public class AppraisalServiceFacade : IAppraisalServiceFacade
    {
        private readonly mhappraisalContext _context;

        public AppraisalServiceFacade()
        {
            _context = new mhappraisalContext();
        }


        public List<string> getStates()
        {
            return _context.States.Select(x => x.Name).ToList();
        }


        public List<string> getManufacturers()
        {
            return _context.Manufacturers.Select(x => x.Manufacturer1).ToList();
        }



        public void GetAppraisalComponents()
        {

        }

        public decimal calculateAppraisalValue(string state, string mfg, int length, int width, int modelYear)
        {
            int area = length * width;
            int age = DateTime.Now.Year - modelYear;

            var areaFactor = _context.AreaFactors.Where(x => x.Area <= area).SingleOrDefault();
            var stateFactor = _context.StateFactors.Where(x => x.State.Name == state).SingleOrDefault();
            var mfgFactor = _context.ManufacturerFactors.Where(x => x.Manufacturer.Manufacturer1 == mfg).SingleOrDefault();
            var ageFactor = _context.AgeFactors.Where(x => x.Age <= age).Take(1).SingleOrDefault();

            decimal? estimatedValue = areaFactor.Factor * area * stateFactor.Factor * mfgFactor.Factor * ageFactor.Factor;

            if (estimatedValue.HasValue)
                return estimatedValue.Value;

            return 0M;
        }


    }
}
