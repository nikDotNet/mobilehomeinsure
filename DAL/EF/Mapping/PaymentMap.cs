using MobileHome.Insure.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.DAL.EF.Mapping
{
    public class PaymentMap : EntityTypeConfiguration<Payment>
    {
        public PaymentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ResponseCode)
                .HasMaxLength(50);

            this.Property(t => t.TransactionId)
                .HasMaxLength(50);

            this.Property(t => t.ApprovalCode)
                .HasMaxLength(10);

            this.Property(t => t.ApprovalMessage)
                .HasMaxLength(150);

            this.Property(t => t.ErrorMessage)
                .HasMaxLength(150);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Payment");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.ResponseCode).HasColumnName("ResponseCode");
            this.Property(t => t.TransactionId).HasColumnName("TransactionId");
            this.Property(t => t.ApprovalCode).HasColumnName("ApprovalCode");
            this.Property(t => t.ApprovalMessage).HasColumnName("ApprovalMessage");
            this.Property(t => t.ErrorMessage).HasColumnName("ErrorMessage");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.RentalQuoteId).HasColumnName("RentalQuoteId");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            this.HasOptional(t => t.Customer)
                .WithMany(t => t.Payments)
                .HasForeignKey(d => d.CustomerId);
            this.HasOptional(t => t.Quote)
                .WithMany(t => t.Payments)
                .HasForeignKey(d => d.RentalQuoteId);

        }
    }
}
