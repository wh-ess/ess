using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class DeliveryMasterMap : EntityTypeConfiguration<DeliveryMaster>
    {
        public DeliveryMasterMap()
        {
            // Primary Key
            this.HasKey(t => t.DeliveryId);

            // Properties
            this.Property(t => t.DeliveryId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.DeliveryProperty)
                .HasMaxLength(50);

            this.Property(t => t.DeliveryAddress)
                .HasMaxLength(50);

            this.Property(t => t.InvoiceNo)
                .HasMaxLength(50);

            this.Property(t => t.CustomerOrderNo)
                .HasMaxLength(50);

            this.Property(t => t.CarNo)
                .HasMaxLength(50);

            this.Property(t => t.Tel)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DeliveryMaster", "Order");
            this.Property(t => t.DeliveryId).HasColumnName("DeliveryId");
            this.Property(t => t.DeliveryDate).HasColumnName("DeliveryDate");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.SalesManId).HasColumnName("SalesManId");
            this.Property(t => t.DeliveryProperty).HasColumnName("DeliveryProperty");
            this.Property(t => t.DeliveryAddress).HasColumnName("DeliveryAddress");
            this.Property(t => t.InvoiceNo).HasColumnName("InvoiceNo");
            this.Property(t => t.CustomerOrderNo).HasColumnName("CustomerOrderNo");
            this.Property(t => t.SubTotal).HasColumnName("SubTotal");
            this.Property(t => t.ValueAddTax).HasColumnName("ValueAddTax");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Receivable).HasColumnName("Receivable");
            this.Property(t => t.Received).HasColumnName("Received");
            this.Property(t => t.LimitDate).HasColumnName("LimitDate");
            this.Property(t => t.CarNo).HasColumnName("CarNo");
            this.Property(t => t.Tel).HasColumnName("Tel");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreateById).HasColumnName("CreateById");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyById).HasColumnName("ModifyById");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");

            // Relationships
            this.HasRequired(t => t.Customer)
                .WithMany(t => t.DeliveryMasters)
                .HasForeignKey(d => d.CustomerId);
            this.HasOptional(t => t.SalesMan)
                .WithMany(t => t.DeliveryMasters)
                .HasForeignKey(d => d.SalesManId);

        }
    }
}
