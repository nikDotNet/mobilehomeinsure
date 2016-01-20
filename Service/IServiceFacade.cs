using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Service
{
    public interface IServiceFacade
    {

        void sendMail(string from, string to, string subject, string message, List<string> bcc = null, bool isOrderMail = false);
    }
}
