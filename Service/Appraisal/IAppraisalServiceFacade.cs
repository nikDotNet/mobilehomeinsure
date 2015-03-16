using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Service.Appraisal
{
    public interface IAppraisalServiceFacade
    {
        Dictionary<int, string> getOptionTypes();
    }
}
