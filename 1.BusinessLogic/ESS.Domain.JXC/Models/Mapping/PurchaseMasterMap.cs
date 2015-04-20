using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class PurchaseMasterMap : EntityTypeConfiguration<PurchaseMaster>
    {
        public PurchaseMasterMap()
        {
            // Primary Key
            this.HasKey(t => t.PurchaseId);

            // Properties
            this.Property(t => t.PurchaseId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.PurchaseProperty)
                .HasMaxLength(10);

            this.Property(t => t.InvoiceNo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PurchaseMaster", "Order");
            this.Property(t => t.PurchaseId).HasColumnName("PurchaseId");
            this.Property(t => t.PurchaseDate).HasColumnName("PurchaseDate");
            this.Property(t => t.SupplierId).HasColumnName("SupplierId");
            this.Property(t => t.PurchaseProperty).HasColumnName("PurchaseProperty");
            this.Property(t => t.InvoiceNo).HasColumnName("InvoiceNo");
            this.Property(t => t.SubTotal).HasColumnName("SubTotal");
            this.Property(t => t.ValueAddTax).HasColumnName("ValueAddTax");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Payable).HasColumnName("Payable");
            this.Property(t => t.Paid).HasColumnName("Paid");
            this.Property(t => t.LimitDate).HasColumnName("LimitDate");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreateById).HasColumnName("CreateById");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyById).HasColumnName("ModifyById");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");

            // Relationships
            this.HasRequired(t => t.Supplier)
                .WithMany(t => t.PurchaseMasters)
                .HasForeignKey(d => d.SupplierId);

        }
    }
}
