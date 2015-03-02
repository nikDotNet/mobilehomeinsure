using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileHome.Insure.DAL;
using MobileHome.Insure.Model;

namespace MobileHome.Insure.Service
{
    public class ServiceFacade : IServiceFacade
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceFacade(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void getStates()
        {
            var obj = _unitOfWork.GetRepository<State>().GetAll().ToList();
        }
    }
}
