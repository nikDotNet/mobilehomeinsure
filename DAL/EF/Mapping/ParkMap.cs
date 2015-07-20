using MobileHome.Insure.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.DAL.EF.Mapping
{
    public class ParkMap : EntityTypeConfiguration<Park>
    {
        public ParkMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Address)
                .HasMaxLength(200);

            this.Property(t => t.City)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.County)
                .HasMaxLength(50);

            this.Property(t => t.Phone)
                .HasMaxLength(50);

            this.Property(t => t.ContactName)
                .HasMaxLength(100);

            this.Property(t => t.Position)
                .HasMaxLength(50);
            
            this.Property(t => t.CreatedBy).HasMaxLength(50);
            
            // Table & Column Mappings
            this.ToTable("Park");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.StateId).HasColumnName("StateId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Zip).HasColumnName("Zip");
            this.Property(t => t.Zip4).HasColumnName("Zip4");
            this.Property(t => t.County).HasColumnName("County");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Spaces).HasColumnName("Spaces");
            this.Property(t => t.ContactName).HasColumnName("ContactName");
            this.Property(t => t.Position).HasColumnName("Position");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            this.HasRequired(t => t.State)
                .WithMany(t => t.Parks)
                .HasForeignKey(d => d.StateId);

        }
    }
}
