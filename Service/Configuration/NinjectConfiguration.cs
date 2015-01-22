using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;

namespace MobileHome.Insure.Service.Configuration
{
   public class NinjectConfiguration : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IServiceFacade>().To<ServiceFacade>();
        }
    }
}
