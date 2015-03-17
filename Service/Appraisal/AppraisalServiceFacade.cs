using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileHome.Insure.DAL.EF;
using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Appraisal;

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


        public Dictionary<int, string> getOptionTypes()
        {
            Dictionary<int, string> optionTypes = new Dictionary<int, string>();
            var listOptionTypes = _context.OptionsTypes.Where(x => x.IsActive == true).ToList();

            foreach (var opt in listOptionTypes)
            {
                optionTypes.Add(opt.Id, opt.Name);
            }
            return optionTypes;
        }


        public List<Manufacturer> getManufacturers()
        {
            return _context.Manufacturers.OrderBy(x => x.Manufacturer1).ToList();
        }



        public void GetAppraisalComponents()
        {

        }

        public decimal calculateAppraisalValue(int state, int mfg, int length, int width, int modelYear, List<int> options, decimal? BrickLinearFootage, decimal? VinylLinearFootage)
        {
            int area = length * width;
            int age = DateTime.Now.Year - modelYear;
            List<decimal> optionsAmount = new List<decimal>();

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
            
            foreach (int option in options)
            {
                //var optionFactor = _context.OptionsFactors.Where(x => x.ManufacturerId == mfg && x.StateId == state && x.OptionsTypeId == option).Take(1);
                var optionFactor = _context.OptionsFactors.Where(x => x.OptionsTypeId == option && x.IsActive == true).Take(1);
                switch (option)
                {
                    case 1: estimatedValue = estimatedValue + (optionFactor.FirstOrDefault().Rate * optionFactor.FirstOrDefault().Factor);
                            break;
                    case 2: estimatedValue = estimatedValue + (optionFactor.FirstOrDefault().Rate * optionFactor.FirstOrDefault().Factor * BrickLinearFootage.Value); // linear foot
                            break;
                    case 3: estimatedValue = estimatedValue + (optionFactor.FirstOrDefault().Rate * optionFactor.FirstOrDefault().Factor * VinylLinearFootage.Value); // linear foot
                            break;
                    case 4:
                    case 5: estimatedValue = estimatedValue + (optionFactor.FirstOrDefault().Rate * optionFactor.FirstOrDefault().Factor * area); // area
                            break;

                }
            }

            

            if (estimatedValue.HasValue)
                return estimatedValue.Value;

            return 0M;
        }

        #region AgeFactor
        public List<AgeFactor> GetAgeFactor()
        {
            return _context.AgeFactors.ToList();
        }

        public void saveAgeFactor(AgeFactor objAgeFactor)
        {
            if (objAgeFactor.Id != 0)
            {
                var existingObj = _context.AgeFactors.Where(x => x.Id == objAgeFactor.Id).FirstOrDefault();
                if (existingObj != null)
                {

                    existingObj.Age = objAgeFactor.Age;
                    existingObj.Factor = objAgeFactor.Factor;
                   _context.Entry(existingObj).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            else
            {
                objAgeFactor.isActive = true;
                objAgeFactor.CreatedOn = DateTime.Now;
                objAgeFactor.CreatedBy = "admin";
                _context.AgeFactors.Add(objAgeFactor);
                _context.SaveChanges();
            }
        }

       

        #endregion

        #region AreaFactor
        public List<AreaFactor> GetAreaFactor()
        {
            return _context.AreaFactors.ToList();
        }

        public void saveAreaFactor(AreaFactor objAreaFactor)
        {
            if (objAreaFactor.Id != 0)
            {
                var existingObj = _context.AreaFactors.Where(x => x.Id == objAreaFactor.Id).SingleOrDefault();
                if (existingObj != null)
                {
                    existingObj.Area = objAreaFactor.Area;
                    existingObj.Factor = objAreaFactor.Factor;
                    _context.Entry(existingObj).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            else
            {
                objAreaFactor.CreatedOn = DateTime.Now;
                objAreaFactor.isActive = true;
                objAreaFactor.CreatedBy = "admin";
                _context.AreaFactors.Add(objAreaFactor);
                _context.SaveChanges();
            }
        }
        
        
        
        #endregion

        #region ManufacturerFactor
        public List<ManufacturerFactor> GetManufacturerFactor()
        {
            return _context.ManufacturerFactors.ToList();
        }

        public void saveManufacturerFactor(ManufacturerFactor objManufacturerFactor)
        {
            if (objManufacturerFactor.Id != 0)
            {
                var existingObj = _context.ManufacturerFactors.Where(x => x.Id == objManufacturerFactor.Id).SingleOrDefault();
                if (existingObj != null)
                {
                    existingObj.ManufacturerId = objManufacturerFactor.ManufacturerId;
                    existingObj.Factor = objManufacturerFactor.Factor;
                    _context.Entry(existingObj).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            else
            {
                objManufacturerFactor.CreatedOn = DateTime.Now;
                objManufacturerFactor.CreatedBy = "admin";
                objManufacturerFactor.isActive = true;
                _context.ManufacturerFactors.Add(objManufacturerFactor);
                _context.SaveChanges();
            }
        }

        #endregion

        #region saveStateFactor
        public List<StateFactor> GetStateFactor()
        {
            return _context.StateFactors.ToList();
        }

        public void saveStateFactor(StateFactor objStateFactor)
        {
            if (objStateFactor.Id != 0)
            {
                var existingObj = _context.StateFactors.Where(x => x.Id == objStateFactor.Id).SingleOrDefault();
                if (existingObj != null)
                {
                    existingObj.StateId = objStateFactor.StateId;
                    existingObj.Factor = objStateFactor.Factor;
                    _context.Entry(existingObj).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            else
            {
                objStateFactor.CreatedBy = "admin";
                objStateFactor.CreatedOn = DateTime.Now;
                objStateFactor.isActive = true;
                _context.StateFactors.Add(objStateFactor);
                _context.SaveChanges();
            }
        }

        #endregion

        public List<Manufacturer> GetManufacturer()
        {
            return _context.Manufacturers.ToList();
        }

        public List<State> GetState()
        {
            return _context.States.ToList();
        }
    }
}
