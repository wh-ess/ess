using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class ReceiveMasterMap : EntityTypeConfiguration<ReceiveMaster>
    {
        public ReceiveMasterMap()
        {
            // Primary Key
            this.HasKey(t => t.ReceiveId);

            // Properties
            this.Property(t => t.ReceiveId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.NoteNo)
                .HasMaxLength(50);

            this.Property(t => t.AccountNo)
                .HasMaxLength(50);

            this.Property(t => t.Type)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("ReceiveMaster", "Order");
            this.Property(t => t.ReceiveId).HasColumnName("ReceiveId");
            this.Property(t => t.ReceiveDate).HasColumnName("ReceiveDate");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.ReceiveCash).HasColumnName("ReceiveCash");
            this.Property(t => t.ReceiveCheck).HasColumnName("ReceiveCheck");
            this.Property(t => t.NoteNo).HasColumnName("NoteNo");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.BankId).HasColumnName("BankId");
            this.Property(t => t.AccountNo).HasColumnName("AccountNo");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.Remittance).HasColumnName("Remittance");
            this.Property(t => t.AdvancePay).HasColumnName("AdvancePay");
            this.Property(t => t.Others).HasColumnName("Others");
            this.Property(t => t.ReceiveAmount).HasColumnName("ReceiveAmount");
            this.Property(t => t.TotalBalance).HasColumnName("TotalBalance");
            this.Property(t => t.AccountAmt).HasColumnName("AccountAmt");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreateById).HasColumnName("CreateById");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyById).HasColumnName("ModifyById");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");

            // Relationships
            this.HasOptional(t => t.Bank)
                .WithMany(t => t.ReceiveMasters)
                .HasForeignKey(d => d.BankId);
            this.HasRequired(t => t.Customer)
                .WithMany(t => t.ReceiveMasters)
                .HasForeignKey(d => d.CustomerId);

        }
    }
}
