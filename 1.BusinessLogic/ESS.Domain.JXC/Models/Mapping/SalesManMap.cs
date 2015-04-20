using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class SalesManMap : EntityTypeConfiguration<SalesMan>
    {
        public SalesManMap()
        {
            // Primary Key
            this.HasKey(t => t.SalesManId);

            // Properties
            this.Property(t => t.ChineseName)
                .HasMaxLength(50);

            this.Property(t => t.EnglishName)
                .HasMaxLength(50);

            this.Property(t => t.ContactPhone)
                .HasMaxLength(50);

            this.Property(t => t.MobilePhone)
                .HasMaxLength(50);

            this.Property(t => t.ContactAddress)
                .HasMaxLength(100);

            this.Property(t => t.EMail)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SalesMan", "Common");
            this.Property(t => t.SalesManId).HasColumnName("SalesManId");
            this.Property(t => t.ChineseName).HasColumnName("ChineseName");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.ContactPhone).HasColumnName("ContactPhone");
            this.Property(t => t.MobilePhone).HasColumnName("MobilePhone");
            this.Property(t => t.ContactAddress).HasColumnName("ContactAddress");
            this.Property(t => t.EMail).HasColumnName("EMail");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreateById).HasColumnName("CreateById");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyById).HasColumnName("ModifyById");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");
        }
    }
}
