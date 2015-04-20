using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class SupplierMap : EntityTypeConfiguration<Supplier>
    {
        public SupplierMap()
        {
            // Primary Key
            this.HasKey(t => t.SupplierId);

            // Properties
            this.Property(t => t.SupplierAttribName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SupplierName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.InvoiceNo)
                .HasMaxLength(50);

            this.Property(t => t.Owner)
                .HasMaxLength(50);

            this.Property(t => t.RocId)
                .HasMaxLength(50);

            this.Property(t => t.Phone1)
                .HasMaxLength(50);

            this.Property(t => t.Phone2)
                .HasMaxLength(50);

            this.Property(t => t.Fax)
                .HasMaxLength(50);

            this.Property(t => t.ContactName1)
                .HasMaxLength(50);

            this.Property(t => t.ContactName2)
                .HasMaxLength(50);

            this.Property(t => t.CompanyAddress)
                .HasMaxLength(100);

            this.Property(t => t.DeliveryAddress)
                .HasMaxLength(100);

            this.Property(t => t.InvoiceAddress)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Supplier", "Common");
            this.Property(t => t.SupplierId).HasColumnName("SupplierId");
            this.Property(t => t.SupplierCode).HasColumnName("SupplierCode");
            this.Property(t => t.SupplierAttribName).HasColumnName("SupplierAttribName");
            this.Property(t => t.SupplierName).HasColumnName("SupplierName");
            this.Property(t => t.InvoiceNo).HasColumnName("InvoiceNo");
            this.Property(t => t.Owner).HasColumnName("Owner");
            this.Property(t => t.RocId).HasColumnName("RocId");
            this.Property(t => t.Phone1).HasColumnName("Phone1");
            this.Property(t => t.Phone2).HasColumnName("Phone2");
            this.Property(t => t.Fax).HasColumnName("Fax");
            this.Property(t => t.ContactName1).HasColumnName("ContactName1");
            this.Property(t => t.ContactName2).HasColumnName("ContactName2");
            this.Property(t => t.CompanyAddress).HasColumnName("CompanyAddress");
            this.Property(t => t.DeliveryAddress).HasColumnName("DeliveryAddress");
            this.Property(t => t.InvoiceAddress).HasColumnName("InvoiceAddress");
            this.Property(t => t.LastPurchaseDate).HasColumnName("LastPurchaseDate");
            this.Property(t => t.PayDays).HasColumnName("PayDays");
            this.Property(t => t.Prepaid).HasColumnName("Prepaid");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreateById).HasColumnName("CreateById");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyById).HasColumnName("ModifyById");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");
        }
    }
}
