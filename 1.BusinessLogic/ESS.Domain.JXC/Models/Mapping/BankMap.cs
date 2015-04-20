using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class BankMap : EntityTypeConfiguration<Bank>
    {
        public BankMap()
        {
            // Primary Key
            this.HasKey(t => t.BankId);

            // Properties
            this.Property(t => t.BankAttribName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.BankName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Bank", "Common");
            this.Property(t => t.BankId).HasColumnName("BankId");
            this.Property(t => t.BankCode).HasColumnName("BankCode");
            this.Property(t => t.BankAttribName).HasColumnName("BankAttribName");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreateById).HasColumnName("CreateById");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyById).HasColumnName("ModifyById");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");
        }
    }
}
