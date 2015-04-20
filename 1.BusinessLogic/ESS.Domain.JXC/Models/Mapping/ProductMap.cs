using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductId);

            // Properties
            this.Property(t => t.ProductCode)
                .HasMaxLength(50);

            this.Property(t => t.ProductName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UnitStock)
                .HasMaxLength(50);

            this.Property(t => t.UnitPackage)
                .HasMaxLength(50);

            this.Property(t => t.ProductColor)
                .HasMaxLength(50);

            this.Property(t => t.ProductSize)
                .HasMaxLength(50);

            this.Property(t => t.WeightUnit)
                .HasMaxLength(50);

            this.Property(t => t.ProductUse)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Product", "Common");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.SafeStock).HasColumnName("SafeStock");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.ProductSpecification).HasColumnName("ProductSpecification");
            this.Property(t => t.UnitStock).HasColumnName("UnitStock");
            this.Property(t => t.UnitPackage).HasColumnName("UnitPackage");
            this.Property(t => t.ProductColor).HasColumnName("ProductColor");
            this.Property(t => t.ProductSize).HasColumnName("ProductSize");
            this.Property(t => t.ProductWeight).HasColumnName("ProductWeight");
            this.Property(t => t.WeightUnit).HasColumnName("WeightUnit");
            this.Property(t => t.ProductUse).HasColumnName("ProductUse");
            this.Property(t => t.CostStandard).HasColumnName("CostStandard");
            this.Property(t => t.PriceStandard).HasColumnName("PriceStandard");
            this.Property(t => t.PriceMember).HasColumnName("PriceMember");
            this.Property(t => t.PriceVip).HasColumnName("PriceVip");
            this.Property(t => t.PriceWholeSale).HasColumnName("PriceWholeSale");
            this.Property(t => t.PriceOther).HasColumnName("PriceOther");
            this.Property(t => t.ActualStock).HasColumnName("ActualStock");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreateById).HasColumnName("CreateById");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyById).HasColumnName("ModifyById");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            this.Property(t => t.ProductClass_ClassId).HasColumnName("ProductClass_ClassId");
            this.Property(t => t.Brand_BrandId).HasColumnName("Brand_BrandId");

            // Relationships
            this.HasOptional(t => t.Brand)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.Brand_BrandId);
            this.HasOptional(t => t.ProductClass)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.ProductClass_ClassId);

        }
    }
}
