using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MobileHome.Insure.DAL.EF.Mapping;
using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Appraisal;
using MobileHome.Insure.Model.Rental;

namespace MobileHome.Insure.DAL.EF
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
        public DbSet<OptionsFactor> OptionsFactors { get; set; }
        public DbSet<OptionsType> OptionsTypes { get; set; }

        public DbSet<Park> Parks { get; set; }

        public DbSet<ParkNotify> ParkNotifies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AgeFactorMap());
            modelBuilder.Configurations.Add(new AreaFactorMap());
            modelBuilder.Configurations.Add(new ManufacturerFactorMap());
            modelBuilder.Configurations.Add(new StateFactorMap());
            modelBuilder.Configurations.Add(new ManufacturerMap());
            modelBuilder.Configurations.Add(new StateMap());
            modelBuilder.Configurations.Add(new OptionsFactorMap() );
            modelBuilder.Configurations.Add(new OptionsTypeMap());
            modelBuilder.Configurations.Add(new ParkMap());
            modelBuilder.Configurations.Add(new ParkNotifyMap());
        }
    }
}
