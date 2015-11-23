using MobileHome.Insure.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.DAL.EF.Mapping
{
    public class ParkSiteMap : EntityTypeConfiguration<ParkSite>
    {
        public ParkSiteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TenantFirstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TenantLastName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PhysicalAddress1)
                .HasMaxLength(150);

            this.Property(t => t.PhysicalAddress2)
                .HasMaxLength(150);

            this.Property(t => t.PhysicalCity)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ParkSites");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParkId).HasColumnName("ParkId");
            this.Property(t => t.SiteNumber).HasColumnName("SiteNumber");
            this.Property(t => t.TenantFirstName).HasColumnName("TenantFirstName");
            this.Property(t => t.TenantLastName).HasColumnName("TenantLastName");
            this.Property(t => t.PhysicalAddress1).HasColumnName("PhysicalAddress1");
            this.Property(t => t.PhysicalAddress2).HasColumnName("PhysicalAddress2");
            this.Property(t => t.PhysicalCity).HasColumnName("PhysicalCity");
            this.Property(t => t.PhysicalStateId).HasColumnName("PhysicalStateId");
            this.Property(t => t.PhysicalZip).HasColumnName("PhysicalZip");
            this.Property(t => t.QuoteId).HasColumnName("QuoteId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");

            // Relationships
            this.HasOptional(t => t.Park)
                .WithMany(t => t.ParkSites)
                .HasForeignKey(d => d.ParkId);
            this.HasOptional(t => t.State)
                .WithMany(t => t.ParkSites)
                .HasForeignKey(d => d.PhysicalStateId);
            this.HasOptional(t => t.Quote)
                .WithMany(t => t.ParkSites)
                .HasForeignKey(d => d.QuoteId);
        }
    }
}
