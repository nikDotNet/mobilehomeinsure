using MobileHome.Insure.Model.Rental;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.DAL.EF.Mapping
{
    public class QuoteMap : EntityTypeConfiguration<Quote>
    {
        public QuoteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ProposalNumber)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Quote", "rental");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProposalNumber).HasColumnName("ProposalNumber");
            this.Property(t => t.PersonalProperty).HasColumnName("PersonalProperty");
            this.Property(t => t.Liability).HasColumnName("Liability");
            this.Property(t => t.Deductible).HasColumnName("Deductible");
            this.Property(t => t.Premium).HasColumnName("Premium");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");

            // Relationships
            this.HasOptional(t => t.Customer)
                .WithMany(t => t.Quotes)
                .HasForeignKey(d => d.CustomerId);
            this.HasOptional(t => t.Company)
                .WithMany(t => t.Quotes)
                .HasForeignKey(d => d.CompanyId);

            
        }
   }
}
