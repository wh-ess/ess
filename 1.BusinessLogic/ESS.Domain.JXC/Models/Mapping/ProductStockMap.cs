using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class ProductStockMap : EntityTypeConfiguration<ProductStock>
    {
        public ProductStockMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.BatchNo)
                .HasMaxLength(10);

            this.Property(t => t.Type)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("ProductStock", "Order");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.BatchNo).HasColumnName("BatchNo");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateById).HasColumnName("CreateById");

            // Relationships
            this.HasRequired(t => t.Product)
                .WithMany(t => t.ProductStocks)
                .HasForeignKey(d => d.ProductId);

        }
    }
}
