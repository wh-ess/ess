using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class ProductClassMap : EntityTypeConfiguration<ProductClass>
    {
        public ProductClassMap()
        {
            // Primary Key
            this.HasKey(t => t.ClassId);

            // Properties
            this.Property(t => t.ClassName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProductClass", "Common");
            this.Property(t => t.ClassId).HasColumnName("ClassId");
            this.Property(t => t.ClassCode).HasColumnName("ClassCode");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.ClassParentId).HasColumnName("ClassParentId");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreateById).HasColumnName("CreateById");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyById).HasColumnName("ModifyById");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");
        }
    }
}
