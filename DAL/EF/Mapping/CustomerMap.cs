using MobileHome.Insure.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.DAL.EF.Mapping
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {

        public CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FirstName)
                .HasMaxLength(70);

            this.Property(t => t.LastName)
                .HasMaxLength(70);

            this.Property(t => t.FirstName2)
                .HasMaxLength(70);

            this.Property(t => t.LastName2)
                .HasMaxLength(70);

            this.Property(t => t.Address)
                .HasMaxLength(200);

            this.Property(t => t.Zip)
                .HasMaxLength(10);

            this.Property(t => t.Phone)
                .HasMaxLength(20);

            this.Property(t => t.Email)
                .HasMaxLength(100);

            this.Property(t => t.City)
                .HasMaxLength(50);

            this.Property(t => t.SiteNumber)
                .HasMaxLength(8);

            this.Property(t => t.CreatedBy).HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Customer");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.FirstName2).HasColumnName("FirstName2");
            this.Property(t => t.LastName2).HasColumnName("LastName2");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Zip).HasColumnName("Zip");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.StateId).HasColumnName("StateId");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.ParkId).HasColumnName("ParkId");
            this.Property(t => t.IsUnsubscribed).HasColumnName("IsUnsubscribed");
            this.Property(t => t.SiteNumber).HasColumnName("SiteNumber");

            // Relationships
            this.HasOptional(t => t.Park)
                .WithMany(t => t.Customers)
                .HasForeignKey(d => d.ParkId);
            this.HasOptional(t => t.State)
                .WithMany(t => t.Customers)
                .HasForeignKey(d => d.StateId);
            this.HasOptional(t => t.User)
                .WithMany(t => t.Customers)
                .HasForeignKey(d => d.UserId);
        }

    }
}
