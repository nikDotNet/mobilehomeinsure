using MobileHome.Insure.DAL.EF.Mapping;
using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Rental;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.DAL.EF
{
    public partial class mhRentalContext : DbContext, IDBContext
    {
        static mhRentalContext()
        {
            Database.SetInitializer<mhappraisalContext>(null);
        }

        public mhRentalContext()
            : base("Name=mhappraisalContext")
        {
        }


        public virtual DbSet<Quote> Quotes { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new QuoteMap());
            modelBuilder.Configurations.Add(new PaymentMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new UserMap());

            //modelBuilder.Entity<Customer>().HasRequired(p => p.User);
        }
    }
}
