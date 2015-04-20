using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class PurchaseDetailMap : EntityTypeConfiguration<PurchaseDetail>
    {
        public PurchaseDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.PurchaseId);

            // Properties
            this.Property(t => t.PurchaseId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.BatchNo)
                .HasMaxLength(10);

            this.Property(t => t.PurchaseMaster_PurchaseId)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("PurchaseDetail", "Order");
            this.Property(t => t.PurchaseId).HasColumnName("PurchaseId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.BatchNo).HasColumnName("BatchNo");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.PurchaseMaster_PurchaseId).HasColumnName("PurchaseMaster_PurchaseId");

            // Relationships
            this.HasRequired(t => t.Product)
                .WithMany(t => t.PurchaseDetails)
                .HasForeignKey(d => d.ProductId);
            this.HasOptional(t => t.PurchaseMaster)
                .WithMany(t => t.PurchaseDetails)
                .HasForeignKey(d => d.PurchaseMaster_PurchaseId);

        }
    }
}
