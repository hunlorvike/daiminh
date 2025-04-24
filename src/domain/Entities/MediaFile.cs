using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;

public class MediaFile : BaseEntity<int>
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long FileSize { get; set; } = 0;
    public string AltText { get; set; } = string.Empty;
    public int? Duration { get; set; }
    public MediaType MediaType { get; set; }
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
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(e => e.AltText).HasColumnName("alt_text").HasMaxLength(255);

        builder.Property(e => e.FileSize).HasColumnName("file_size");
        builder.Property(e => e.Duration).HasColumnName("duration");

        builder.Property(e => e.MediaType)
            .HasColumnName("media_type")
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(MediaType.Image);

        builder.HasIndex(e => e.FilePath).HasDatabaseName("idx_media_files_file_path").IsUnique();
    }
}