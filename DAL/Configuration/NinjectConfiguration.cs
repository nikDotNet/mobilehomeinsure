using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using DAL.EF;
using Ninject.Modules;

namespace DAL.Configuration
{
   public class NinjectConfiguration :NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IDBContext>().To<EFDBContext>();
            Kernel.Bind<IRepository>().To<EFRepository>();
            Kernel.Bind<IUnitOfWork>().To<EFUnitOfWork>();
        }
    }
}
