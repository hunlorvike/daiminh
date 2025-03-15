using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class Folder : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public int? ParentId { get; set; }

    // Navigation properties
    public Folder? Parent { get; set; }
    public virtual ICollection<Folder>? Children { get; set; }
    public virtual ICollection<MediaFile>? MediaFiles { get; set; }
}

public class FolderConfiguration : BaseEntityConfiguration<Folder, int>
{
    public override void Configure(EntityTypeBuilder<Folder> builder)
    {
        base.Configure(builder);

        builder.ToTable("folders");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Path).HasColumnName("path").IsRequired().HasMaxLength(255);
        builder.Property(e => e.ParentId).HasColumnName("parent_id");

        builder.HasOne(e => e.Parent)
            .WithMany(e => e!.Children)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_folders_parent_id");

        builder.HasIndex(e => e.Path).HasDatabaseName("idx_folders_path");
    }
}