using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileHome.Insure.Model.Appraisal;

namespace MobileHome.Insure.DAL.EF.Mapping
{

    public class OptionsFactorMap : EntityTypeConfiguration<OptionsFactor>
    {
        public OptionsFactorMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("OptionsFactor", "appraisal");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OptionsTypeId).HasColumnName("OptionsTypeId");
            this.Property(t => t.StateId).HasColumnName("StateId");
            this.Property(t => t.ManufacturerId).HasColumnName("ManufacturerId");
            this.Property(t => t.Rate).HasColumnName("Rate");
            this.Property(t => t.Factor).HasColumnName("Factor");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            this.HasRequired(t => t.Manufacturer)
                .WithMany(t => t.OptionsFactors)
                .HasForeignKey(d => d.ManufacturerId);
            this.HasRequired(t => t.OptionsType)
                .WithMany(t => t.OptionsFactors)
                .HasForeignKey(d => d.OptionsTypeId);
            this.HasRequired(t => t.State)
                .WithMany(t => t.OptionsFactors)
                .HasForeignKey(d => d.StateId);

        }
    }
}
