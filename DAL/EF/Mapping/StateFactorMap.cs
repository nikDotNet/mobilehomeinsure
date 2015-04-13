using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MobileHome.Insure.Model.Appraisal;

namespace MobileHome.Insure.DAL.EF.Mapping
{
    public class StateFactorMap : EntityTypeConfiguration<StateFactor>
    {
        public StateFactorMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CreatedBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("StateFactor", "appraisal");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.StateId).HasColumnName("StateId");
            this.Property(t => t.Factor).HasColumnName("Factor");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.isActive).HasColumnName("isActive");

            // Relationships
            this.HasRequired(t => t.State)
                .WithMany(t => t.StateFactors)
                .HasForeignKey(d => d.StateId);

        }
    }
}
