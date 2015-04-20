using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class ReceiveDetailMap : EntityTypeConfiguration<ReceiveDetail>
    {
        public ReceiveDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.ReceiveId);

            // Properties
            this.Property(t => t.ReceiveId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.DeliveryId)
                .HasMaxLength(128);

            this.Property(t => t.ReceiveMaster_ReceiveId)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("ReceiveDetail", "Order");
            this.Property(t => t.ReceiveId).HasColumnName("ReceiveId");
            this.Property(t => t.DeliveryId).HasColumnName("DeliveryId");
            this.Property(t => t.Balance).HasColumnName("Balance");
            this.Property(t => t.ReceiveMaster_ReceiveId).HasColumnName("ReceiveMaster_ReceiveId");

            // Relationships
            this.HasOptional(t => t.DeliveryMaster)
                .WithMany(t => t.ReceiveDetails)
                .HasForeignKey(d => d.DeliveryId);
            this.HasOptional(t => t.ReceiveMaster)
                .WithMany(t => t.ReceiveDetails)
                .HasForeignKey(d => d.ReceiveMaster_ReceiveId);

        }
    }
}
