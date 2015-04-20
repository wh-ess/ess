using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class FavoriteMap : EntityTypeConfiguration<Favorite>
    {
        public FavoriteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Url)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Icon)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Favorite", "Foundation");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.AddTime).HasColumnName("AddTime");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.Icon).HasColumnName("Icon");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Favorites)
                .HasForeignKey(d => d.UserId);

        }
    }
}
