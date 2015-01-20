using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DAL.EF.Mapping;
using Model;
using Model.Appraisal;

namespace DAL.EF
{
    public partial class mhappraisalContext : DbContext, IDBContext
    {
        static mhappraisalContext()
        {
            Database.SetInitializer<mhappraisalContext>(null);
        }

        public mhappraisalContext()
            : base("Name=mhappraisalContext")
        {
        }

        public DbSet<AgeFactor> AgeFactors { get; set; }
        public DbSet<AreaFactor> AreaFactors { get; set; }
        public DbSet<ManufacturerFactor> ManufacturerFactors { get; set; }
        public DbSet<StateFactor> StateFactors { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<State> States { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AgeFactorMap());
            modelBuilder.Configurations.Add(new AreaFactorMap());
            modelBuilder.Configurations.Add(new ManufacturerFactorMap());
            modelBuilder.Configurations.Add(new StateFactorMap());
            modelBuilder.Configurations.Add(new ManufacturerMap());
            modelBuilder.Configurations.Add(new StateMap());
        }
    }
}
