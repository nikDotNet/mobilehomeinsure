using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileHome.Insure.Model.Appraisal;

namespace MobileHome.Insure.DAL.EF.Mapping
{
    public class OptionsTypeMap : EntityTypeConfiguration<OptionsType>
    {
        public OptionsTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .HasMaxLength(100);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("OptionsType", "appraisal");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
