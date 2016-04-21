using MobileHome.Insure.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.DAL.EF.Mapping
{
    public class ZipInfoMap : EntityTypeConfiguration<ZipInfo>
    {
        public ZipInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("ZipInfo", "dbo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.CityPreferred).HasColumnName("CityPreferred");
            this.Property(t => t.County).HasColumnName("County");
            this.Property(t => t.CountyNumber).HasColumnName("CountyNumber");
            this.Property(t => t.StateAbbr).HasColumnName("StateAbbr");
            this.Property(t => t.Zip).HasColumnName("Zip");
            this.Property(t => t.ZipKey).HasColumnName("ZipKey");
            this.Property(t => t.TerritoryNumber).HasColumnName("TerritoryNumber");

        }
    }
}