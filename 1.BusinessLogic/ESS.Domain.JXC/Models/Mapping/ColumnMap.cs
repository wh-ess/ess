using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class ColumnMap : EntityTypeConfiguration<Column>
    {
        public ColumnMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ModuleNo, t.Name });

            // Properties
            this.Property(t => t.ModuleNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Text)
                .HasMaxLength(50);

            this.Property(t => t.Type)
                .HasMaxLength(50);

            this.Property(t => t.SourceTableName)
                .HasMaxLength(50);

            this.Property(t => t.SourceTableIDField)
                .HasMaxLength(50);

            this.Property(t => t.SourceTableTextField)
                .HasMaxLength(50);

            this.Property(t => t.SourceTableParentIDField)
                .HasMaxLength(50);

            this.Property(t => t.Align)
                .HasMaxLength(50);

            this.Property(t => t.Group)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Columns", "Foundation");
            this.Property(t => t.ModuleNo).HasColumnName("ModuleNo");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.IsInPrimaryKey).HasColumnName("IsInPrimaryKey");
            this.Property(t => t.IsInForeignKey).HasColumnName("IsInForeignKey");
            this.Property(t => t.IsNullable).HasColumnName("IsNullable");
            this.Property(t => t.IsAutoKey).HasColumnName("IsAutoKey");
            this.Property(t => t.IsTreeColumn).HasColumnName("IsTreeColumn");
            this.Property(t => t.SourceTableName).HasColumnName("SourceTableName");
            this.Property(t => t.SourceTableIDField).HasColumnName("SourceTableIDField");
            this.Property(t => t.SourceTableTextField).HasColumnName("SourceTableTextField");
            this.Property(t => t.SourceTableParentIDField).HasColumnName("SourceTableParentIDField");
            this.Property(t => t.Align).HasColumnName("Align");
            this.Property(t => t.InList).HasColumnName("InList");
            this.Property(t => t.ListWidth).HasColumnName("ListWidth");
            this.Property(t => t.InSearch).HasColumnName("InSearch");
            this.Property(t => t.Search_NewLine).HasColumnName("Search_NewLine");
            this.Property(t => t.InForm).HasColumnName("InForm");
            this.Property(t => t.NewLine).HasColumnName("NewLine");
            this.Property(t => t.Group).HasColumnName("Group");
            this.Property(t => t.Index).HasColumnName("Index");
        }
    }
}
