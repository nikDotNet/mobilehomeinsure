using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MobileHome.Insure.Model.Appraisal;

namespace MobileHome.Insure.DAL.EF.Mapping
{
    public class AreaFactorMap : EntityTypeConfiguration<AreaFactor>
    {
        public AreaFactorMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CreatedBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("AreaFactor", "appraisal");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Area).HasColumnName("Area");
            this.Property(t => t.Factor).HasColumnName("Factor");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.isActive).HasColumnName("isActive");
        }
    }
}
