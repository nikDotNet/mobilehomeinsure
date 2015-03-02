using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MobileHome.Insure.Model.Appraisal;
using MobileHome.Insure.Model;

namespace MobileHome.Insure.DAL.EF
{
    class EFDBContext : DbContext 
    {
        public EFDBContext()
          :base("Name=EFDbContext")
      {
 
      }
 
      public DbSet<AgeFactor> AgeFactors { get; set; }
      public DbSet<AreaFactor> AreaFactors { get; set; }
      public DbSet<ManufacturerFactor> ManufacturerFactors { get; set; }
      public DbSet<StateFactor> StateFactors { get; set; }
      public DbSet<Manufacturer> Manufacturers { get; set; }
      public DbSet<State> States { get; set; }
      
 
      public override int SaveChanges()
      {
          var modifiedEntries = ChangeTracker.Entries()
              .Where(x => x.Entity is IAuditableEntity
                  && (x.State == System.Data.Entity.EntityState.Added || x.State == System.Data.Entity.EntityState.Modified));
 
          foreach (var entry in modifiedEntries)
          {
              IAuditableEntity entity = entry.Entity as IAuditableEntity;
              if (entity != null)
              {
                  string identityName = Thread.CurrentPrincipal.Identity.Name;
                  DateTime now = DateTime.UtcNow;
 
                  if (entry.State == System.Data.Entity.EntityState.Added)
                  {
                      entity.CreatedBy = identityName;
                      entity.CreatedDate = now;
                  }
                  else {
                      base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                      base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;                   
                  }
 
                  entity.UpdatedBy = identityName;
                  entity.UpdatedDate = now;
              }
          }
 
          return base.SaveChanges();
      }
    }
}
