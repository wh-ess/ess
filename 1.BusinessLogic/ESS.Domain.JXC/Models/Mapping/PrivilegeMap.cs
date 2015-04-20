using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class PrivilegeMap : EntityTypeConfiguration<Privilege>
    {
        public PrivilegeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.RoleId, t.ModuleNo, t.ActionName });

            // Properties
            this.Property(t => t.RoleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ModuleNo)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.ActionName)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("Privilege", "Foundation");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.ModuleNo).HasColumnName("ModuleNo");
            this.Property(t => t.ActionName).HasColumnName("ActionName");
            this.Property(t => t.CreateById).HasColumnName("CreateById");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyById).HasColumnName("ModifyById");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");

            // Relationships
            this.HasRequired(t => t.Role)
                .WithMany(t => t.Privileges)
                .HasForeignKey(d => d.RoleId);

        }
    }
}
