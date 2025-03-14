using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class MediaFile : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Extension { get; set; } = string.Empty;
    public int? FolderId { get; set; }
    public virtual Folder? Folder { get; set; }
}

public class MediaFileConfiguration : BaseEntityConfiguration<MediaFile, int>
{
    public override void Configure(EntityTypeBuilder<MediaFile> builder)
    {
        base.Configure(builder);
        builder.ToTable("media_files");
        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Path).HasColumnName("path").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Url).HasColumnName("url").IsRequired().HasMaxLength(255);
        builder.Property(e => e.MimeType).HasColumnName("mime_type").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Size).HasColumnName("size").IsRequired();
        builder.Property(e => e.Extension).HasColumnName("extension").IsRequired().HasMaxLength(10);
        builder.Property(e => e.FolderId).HasColumnName("folder_id");
        builder.HasOne(e => e.Folder)
            .WithMany(e => e!.MediaFiles)
            .HasForeignKey(e => e.FolderId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_media_files_folder_id");
        builder.HasIndex(e => e.Path).HasDatabaseName("idx_media_files_path");
    }
}