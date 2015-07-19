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
    public class UserMap : EntityTypeConfiguration<User>
    {

        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.EmailId).HasMaxLength(100);
            this.Property(t => t.Password).HasMaxLength(100);
            this.Property(t => t.CreatedBy).HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Users");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EmailId).HasColumnName("EmailId");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
        }

    }
}
