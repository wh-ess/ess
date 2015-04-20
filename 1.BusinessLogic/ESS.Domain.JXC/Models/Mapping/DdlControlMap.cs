using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class DdlControlMap : EntityTypeConfiguration<DdlControl>
    {
        public DdlControlMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.FieldText)
                .HasMaxLength(50);

            this.Property(t => t.FieldValue)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DdlControl", "Foundation");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.FieldText).HasColumnName("FieldText");
            this.Property(t => t.FieldValue).HasColumnName("FieldValue");
        }
    }
}
