using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class ChangeMasterMap : EntityTypeConfiguration<ChangeMaster>
    {
        public ChangeMasterMap()
        {
            // Primary Key
            this.HasKey(t => t.ChangeId);

            // Properties
            this.Property(t => t.ChangeId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("ChangeMaster", "Order");
            this.Property(t => t.ChangeId).HasColumnName("ChangeId");
            this.Property(t => t.ChangeDate).HasColumnName("ChangeDate");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreateById).HasColumnName("CreateById");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyById).HasColumnName("ModifyById");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");
        }
    }
}
