using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class MenuMap : EntityTypeConfiguration<Menu>
    {
        public MenuMap()
        {
            // Primary Key
            this.HasKey(t => t.MenuId);
            
            this.Property(t => t.MenuNo)
                .HasMaxLength(50);

            this.Property(t => t.Title)
                .HasMaxLength(200);

            this.Property(t => t.Url)
                .HasMaxLength(200);

            this.Property(t => t.Icon)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Menu", "Foundation");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.MenuNo).HasColumnName("MenuNo");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.MenuParentId).HasColumnName("MenuParentId");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.Enabled).HasColumnName("Enabled");
            this.Property(t => t.Seq).HasColumnName("Seq");
            this.Property(t => t.Icon).HasColumnName("Icon");
        }
    }
}
