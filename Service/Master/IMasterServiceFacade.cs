using MobileHome.Insure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Service.Master
{
    public interface IMasterServiceFacade
    {
        #region State Operations
        List<State> GetStates();
        #endregion

        #region Pack Operations
        List<Park> GetParks();

        Park GetParkById(int id);

        List<Park> FindParkByZip(int zip);

        void SavePark(Park parkObj, bool toDelete = false);

        void SavePark(List<Park> importParks);
        #endregion
    }
}
