using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;
using shared.Models;

namespace domain.Entities;

public class MediaFile : BaseEntity<int>
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
    public string MediumSizePath { get; set; } = string.Empty;
    public string LargeSizePath { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long FileSize { get; set; } = 0;
    public string AltText { get; set; } = string.Empty;

    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Duration { get; set; }

    public MediaType MediaType { get; set; }

    // Navigation properties
    public int? FolderId { get; set; }
    public virtual MediaFolder? MediaFolder { get; set; }
}

public class MediaFileConfiguration : BaseEntityConfiguration<MediaFile, int>
{
    public override void Configure(EntityTypeBuilder<MediaFile> builder)
    {
        base.Configure(builder);
        builder.ToTable("media_files");
        builder.Property(e => e.FileName).HasColumnName("file_name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.OriginalFileName).HasColumnName("original_file_name").HasMaxLength(255);
        builder.Property(e => e.MimeType).HasColumnName("mime_type").HasMaxLength(100);
        builder.Property(e => e.FileExtension).HasColumnName("file_extension").HasMaxLength(10);
        builder.Property(e => e.FilePath).HasColumnName("file_path").HasMaxLength(255);
        builder.Property(e => e.ThumbnailPath).HasColumnName("thumbnail_path").HasMaxLength(255);
        builder.Property(e => e.MediumSizePath).HasColumnName("medium_size_path").HasMaxLength(255);
        builder.Property(e => e.LargeSizePath).HasColumnName("large_size_path").HasMaxLength(255);
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(e => e.FileSize).HasColumnName("file_size");
        builder.Property(e => e.AltText).HasColumnName("alt_text").HasMaxLength(255);
        builder.Property(e => e.Width).HasColumnName("width");
        builder.Property(e => e.Height).HasColumnName("height");
        builder.Property(e => e.Duration).HasColumnName("duration");
        builder.Property(e => e.MediaType).HasColumnName("media_type").HasConversion<int>();
        builder.Property(e => e.FolderId).HasColumnName("folder_id");
        builder.HasOne(e => e.MediaFolder)
            .WithMany(e => e!.Files)
            .HasForeignKey(e => e.FolderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}