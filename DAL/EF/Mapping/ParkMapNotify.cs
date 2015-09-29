using MobileHome.Insure.Model;
using MobileHome.Insure.Model.Rental;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.DAL.EF.Mapping
{
    public class ParkNotifyMap : EntityTypeConfiguration<ParkNotify>
    {
        public ParkNotifyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.Zip)
                .IsRequired()
                .HasMaxLength(10);
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.IsNotified).HasColumnName("IsNotified");

            this.ToTable("ParkNotify");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Zip).HasColumnName("Zip");
            this.Property(t => t.IsNotified).HasColumnName("IsNotified");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
        }
    }
}
