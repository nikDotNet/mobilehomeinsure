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
            //this.Property(t => t.Name)
            //    .IsRequired()
            //    .HasMaxLength(100);

            //this.Property(t => t.Address)
            //    .HasMaxLength(200);

            //this.Property(t => t.City)
            //    .IsRequired()
            //    .HasMaxLength(100);

            //this.Property(t => t.County)
            //    .HasMaxLength(50);

            //this.Property(t => t.Phone)
            //    .HasMaxLength(50);

            //this.Property(t => t.ContactName)
            //    .HasMaxLength(100);

            //this.Property(t => t.Position)
            //    .HasMaxLength(50);

            //this.Property(t => t.CreatedBy).HasMaxLength(50);

            //// Table & Column Mappings
            //this.ToTable("Park");
            //this.Property(t => t.Id).HasColumnName("Id");
            //this.Property(t => t.StateId).HasColumnName("StateId");
            //this.Property(t => t.Name).HasColumnName("Name");
            //this.Property(t => t.Address).HasColumnName("Address");
            //this.Property(t => t.City).HasColumnName("City");
            //this.Property(t => t.Zip).HasColumnName("Zip");
            //this.Property(t => t.Zip4).HasColumnName("Zip4");
            //this.Property(t => t.County).HasColumnName("County");
            //this.Property(t => t.Phone).HasColumnName("Phone");
            //this.Property(t => t.Spaces).HasColumnName("Spaces");
            //this.Property(t => t.ContactName).HasColumnName("ContactName");
            //this.Property(t => t.Position).HasColumnName("Position");

            //// Relationships
            //this.HasRequired(t => t.State)
            //    .WithMany(t => t.Parks)
            //    .HasForeignKey(d => d.StateId);



            // Properties
            this.Property(t => t.ParkName)
                .IsRequired()
                .HasMaxLength(100);

            //Physical Address 
            this.Property(t => t.PhysicalAddress)
                .HasMaxLength(200);

            this.Property(t => t.PhysicalAddress2)
                .HasMaxLength(200);

            this.Property(t => t.PhysicalCity)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PhysicalCounty)
                .HasMaxLength(50);


            //Office attributes
            this.Property(t => t.OfficePhone)
                .HasMaxLength(50);

            this.Property(t => t.OfficeFax)
                .HasMaxLength(50);

            this.Property(t => t.OfficeMail)
                .HasMaxLength(50);

            this.Property(t => t.Website)
                .HasMaxLength(100);


            this.Property(t => t.Contact)
                .HasMaxLength(100);

            this.Property(t => t.Position)
                .HasMaxLength(50);


            //Mailing Address 
            this.Property(t => t.MailingAddress)
                .HasMaxLength(200);

            this.Property(t => t.MailingAddress2)
                .HasMaxLength(200);

            this.Property(t => t.MailingCity)
                .IsRequired()
                .HasMaxLength(100);


            this.Property(t => t.CreatedBy).HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Park");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParkName).HasColumnName("ParkName");

            //physical address
            this.Property(t => t.PhysicalAddress).HasColumnName("PhysicalAddress");
            this.Property(t => t.PhysicalAddress2).HasColumnName("PhysicalAddress2");
            this.Property(t => t.PhysicalCity).HasColumnName("PhysicalCity");
            this.Property(t => t.PhysicalZip).HasColumnName("PhysicalZip");
            this.Property(t => t.PhysicalCounty).HasColumnName("PhysicalCounty");
            this.Property(t => t.PhysicalStateId).HasColumnName("PhysicalStateId");
            this.Ignore(p => p.PhysicalCsvState);

            //Office attributes
            this.Property(t => t.OfficePhone).HasColumnName("OfficePhone");
            this.Property(t => t.OfficeFax).HasColumnName("OfficeFax");
            this.Property(t => t.OfficeMail).HasColumnName("OfficeMail");
            this.Property(t => t.Website).HasColumnName("Website");


            this.Property(t => t.SpacesToRent).HasColumnName("SpacesToRent");
            this.Property(t => t.SpacesToOwn).HasColumnName("SpacesToOwn");
            this.Property(t => t.Contact).HasColumnName("Contact");
            this.Property(t => t.Position).HasColumnName("Position");

            //Mailing address
            this.Property(t => t.MailingAddress).HasColumnName("MailingAddress");
            this.Property(t => t.MailingAddress2).HasColumnName("MailingAddress2");
            this.Property(t => t.MailingCity).HasColumnName("MailingCity");
            this.Property(t => t.MailingZip).HasColumnName("MailingZip");
            this.Property(t => t.MailingStateId).HasColumnName("MailingStateId");
            this.Ignore(p => p.MailingCsvState);


            this.Property(t => t.CreatedDate).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            this.HasRequired(t => t.PhysicalState)
                .WithMany(t => t.PhysicalParks)
                .HasForeignKey(d => d.PhysicalStateId);

            this.HasRequired(t => t.MailingState)
                .WithMany(t => t.MailingParks)
                .HasForeignKey(d => d.MailingStateId);
        }
    }
}
