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
    public string? Description { get; set; }
    public long FileSize { get; set; } = 0;
    public string? AltText { get; set; }
    public int? Duration { get; set; }
    public MediaType MediaType { get; set; }
}

public class MediaFileConfiguration : BaseEntityConfiguration<MediaFile, int>
{
    public override void Configure(EntityTypeBuilder<MediaFile> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.FileName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.OriginalFileName).HasMaxLength(255);
        builder.Property(e => e.MimeType).HasMaxLength(100);
        builder.Property(e => e.FileExtension).HasMaxLength(10);

        builder.Property(e => e.FilePath).HasColumnType("TEXT").IsRequired();

        builder.Property(e => e.Description).HasMaxLength(255);
        builder.Property(e => e.AltText).HasMaxLength(255);

        builder.Property(e => e.FileSize);
        builder.Property(e => e.Duration);

        builder.Property(e => e.MediaType)
            .IsRequired()
            .HasDefaultValue(MediaType.Image);
    }
}