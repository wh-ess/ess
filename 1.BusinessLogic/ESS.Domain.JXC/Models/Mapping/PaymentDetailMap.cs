using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class PaymentDetailMap : EntityTypeConfiguration<PaymentDetail>
    {
        public PaymentDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.PaymentId);

            // Properties
            this.Property(t => t.PaymentId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.PurchaseId)
                .HasMaxLength(128);

            this.Property(t => t.PaymentMaster_PaymentId)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("PaymentDetail", "Order");
            this.Property(t => t.PaymentId).HasColumnName("PaymentId");
            this.Property(t => t.PurchaseId).HasColumnName("PurchaseId");
            this.Property(t => t.Balance).HasColumnName("Balance");
            this.Property(t => t.PaymentMaster_PaymentId).HasColumnName("PaymentMaster_PaymentId");

            // Relationships
            this.HasOptional(t => t.PaymentMaster)
                .WithMany(t => t.PaymentDetails)
                .HasForeignKey(d => d.PaymentMaster_PaymentId);
            this.HasOptional(t => t.PurchaseMaster)
                .WithMany(t => t.PaymentDetails)
                .HasForeignKey(d => d.PurchaseId);

        }
    }
}
