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
            this.Property(t => t.Name)
                .HasMaxLength(100);

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


            // Table & Column Mappings
            this.ToTable("Customer");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Zip).HasColumnName("Zip");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.StateId).HasColumnName("StateId");
            this.Property(t => t.City).HasColumnName("City");

            // Relationships
            this.HasOptional(t => t.State)
                .WithMany(t => t.Customers)
                .HasForeignKey(d => d.StateId);
            this.HasOptional(t => t.User)
                .WithMany(t => t.Customers)
                .HasForeignKey(d => d.UserId);
        }

    }
}
