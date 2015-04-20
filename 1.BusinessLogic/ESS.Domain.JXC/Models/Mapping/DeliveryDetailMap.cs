using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class DeliveryDetailMap : EntityTypeConfiguration<DeliveryDetail>
    {
        public DeliveryDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.DeliveryId);

            // Properties
            this.Property(t => t.DeliveryId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.BatchNo)
                .HasMaxLength(50);

            this.Property(t => t.DeliveryMaster_DeliveryId)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("DeliveryDetail", "Order");
            this.Property(t => t.DeliveryId).HasColumnName("DeliveryId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.BatchNo).HasColumnName("BatchNo");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.UnSent).HasColumnName("UnSent");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.DeliveryMaster_DeliveryId).HasColumnName("DeliveryMaster_DeliveryId");

            // Relationships
            this.HasRequired(t => t.Product)
                .WithMany(t => t.DeliveryDetails)
                .HasForeignKey(d => d.ProductId);
            this.HasOptional(t => t.DeliveryMaster)
                .WithMany(t => t.DeliveryDetails)
                .HasForeignKey(d => d.DeliveryMaster_DeliveryId);

        }
    }
}
