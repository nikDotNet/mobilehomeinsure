﻿using MobileHome.Insure.DAL.EF.Mapping;
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
            this.Configuration.ProxyCreationEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
        }


        public virtual DbSet<Quote> Quotes { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Park> Parks { get; set; }
        public virtual DbSet<ParkSite> ParkSites { get; set; }
        public virtual DbSet<ParkNotify> ParkNotifies { get; set; }
        public virtual DbSet<ZipInfo> ZipInfo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new QuoteMap());
            modelBuilder.Configurations.Add(new PaymentMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new ParkMap());
            modelBuilder.Configurations.Add(new ParkNotifyMap());
            modelBuilder.Configurations.Add(new ParkSiteMap());
            modelBuilder.Configurations.Add(new ZipInfoMap());
        }
    }
}
