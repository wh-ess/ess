using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class ChangeDetailMap : EntityTypeConfiguration<ChangeDetail>
    {
        public ChangeDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.ChangeId);

            // Properties
            this.Property(t => t.ChangeId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.ChangeMaster_ChangeId)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("ChangeDetail", "Order");
            this.Property(t => t.ChangeId).HasColumnName("ChangeId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.ChangeMaster_ChangeId).HasColumnName("ChangeMaster_ChangeId");

            // Relationships
            this.HasOptional(t => t.ChangeMaster)
                .WithMany(t => t.ChangeDetails)
                .HasForeignKey(d => d.ChangeMaster_ChangeId);

        }
    }
}
