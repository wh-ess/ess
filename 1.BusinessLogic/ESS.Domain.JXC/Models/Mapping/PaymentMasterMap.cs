using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class PaymentMasterMap : EntityTypeConfiguration<PaymentMaster>
    {
        public PaymentMasterMap()
        {
            // Primary Key
            this.HasKey(t => t.PaymentId);

            // Properties
            this.Property(t => t.PaymentId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.NoteNo)
                .HasMaxLength(50);

            this.Property(t => t.AccountNo)
                .HasMaxLength(50);

            this.Property(t => t.Type)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("PaymentMaster", "Order");
            this.Property(t => t.PaymentId).HasColumnName("PaymentId");
            this.Property(t => t.PayDate).HasColumnName("PayDate");
            this.Property(t => t.SupplierId).HasColumnName("SupplierId");
            this.Property(t => t.PayCash).HasColumnName("PayCash");
            this.Property(t => t.PayCheck).HasColumnName("PayCheck");
            this.Property(t => t.NoteNo).HasColumnName("NoteNo");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.BankId).HasColumnName("BankId");
            this.Property(t => t.AccountNo).HasColumnName("AccountNo");
            this.Property(t => t.AccountAmt).HasColumnName("AccountAmt");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.Remittance).HasColumnName("Remittance");
            this.Property(t => t.Prepayment).HasColumnName("Prepayment");
            this.Property(t => t.Others).HasColumnName("Others");
            this.Property(t => t.PayAmount).HasColumnName("PayAmount");
            this.Property(t => t.TotalBalance).HasColumnName("TotalBalance");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreateById).HasColumnName("CreateById");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyById).HasColumnName("ModifyById");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");

            // Relationships
            this.HasRequired(t => t.Supplier)
                .WithMany(t => t.PaymentMasters)
                .HasForeignKey(d => d.SupplierId);

        }
    }
}
