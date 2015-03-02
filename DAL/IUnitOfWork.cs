using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileHome.Insure.Model.Base;

namespace MobileHome.Insure.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        /// 
        int Commit();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    }
}
