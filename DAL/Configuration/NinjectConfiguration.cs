using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileHome.Insure.DAL;
using MobileHome.Insure.DAL.EF;
using Ninject.Modules;

namespace MobileHome.Insure.DAL.Configuration
{
   public class NinjectConfiguration :NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<DbContext>().To<EFDBContext>();
            Kernel.Bind(typeof(IRepository<>)).To(typeof(EFRepository<>));
            Kernel.Bind<IUnitOfWork>().To<EFUnitOfWork>();
        }
    }
}
