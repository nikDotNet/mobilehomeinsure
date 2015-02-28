using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileHome.Insure.DAL.EF;
using MobileHome.Insure.Model;

namespace MobileHome.Insure.Service.Appraisal
{
    public class AppraisalServiceFacade : IAppraisalServiceFacade
    {
        private readonly mhappraisalContext _context;

        public AppraisalServiceFacade()
        {
            _context = new mhappraisalContext();
        }


        public List<State> getStates()
        {
            return _context.States. OrderBy(x => x.Name).ToList();
        }


        public List<Manufacturer> getManufacturers()
        {
            return _context.Manufacturers.OrderBy(x => x.Manufacturer1).ToList();
        }



        public void GetAppraisalComponents()
        {

        }

        public decimal calculateAppraisalValue(int state, int mfg, int length, int width, int modelYear)
        {
            int area = length * width;
            int age = DateTime.Now.Year - modelYear;
            decimal? agefactor = 0M;
            decimal? areafactor = 0M;
            var stateFactor = _context.StateFactors.Where(x => x.Id == state).SingleOrDefault();
            var mfgFactor = _context.ManufacturerFactors.Where(x => x.Id == mfg).SingleOrDefault();
            var areaFactor = _context.AreaFactors.Where(x => x.Area >= area).Take(1);
            var ageFactor = _context.AgeFactors.ToList().Where(x => x.Age >= age).Take(1);
            if (areaFactor.Count() < 1)
                areafactor = 0M;
            else
                areafactor = areaFactor.FirstOrDefault().Factor;

            if(ageFactor.Count() > 0)
                agefactor = ageFactor.FirstOrDefault().Factor;
            else
                agefactor = 0M;


            decimal? estimatedValue = areafactor * area * stateFactor.Factor * mfgFactor.Factor * agefactor;

            if (estimatedValue.HasValue)
                return estimatedValue.Value;

            return 0M;
        }


    }
}
