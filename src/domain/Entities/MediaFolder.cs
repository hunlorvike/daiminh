using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class MediaFolder : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? ParentId { get; set; }

    // Navigation properties
    public virtual MediaFolder? Parent { get; set; }

    public virtual ICollection<MediaFolder> Children { get; set; }

    public virtual ICollection<MediaFile> Files { get; set; }
}

public class MediaFolderConfiguration : BaseEntityConfiguration<MediaFolder, int>
{
    public override void Configure(EntityTypeBuilder<MediaFolder> builder)
    {
        base.Configure(builder);
        builder.ToTable("media_folders");
        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(e => e.ParentId).HasColumnName("parent_id");
        builder.HasOne(e => e.Parent)
            .WithMany(e => e!.Children)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.Files)
            .WithOne(e => e.MediaFolder)
            .HasForeignKey(e => e.FolderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}   