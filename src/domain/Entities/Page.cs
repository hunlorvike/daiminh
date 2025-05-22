using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;

public class Page : SeoEntity<int>
{
    public string Title { get; set; } = string.Empty; // Tiêu đề trang
    public string Slug { get; set; } = string.Empty; // Đường dẫn thân thiện
    public string? Content { get; set; } // Nội dung trang
    public PublishStatus Status { get; set; } = PublishStatus.Draft;
    public DateTime? PublishedAt { get; set; } // Ngày xuất bản
}
public class PageConfiguration : BaseEntityConfiguration<Page, int>
{
    public override void Configure(EntityTypeBuilder<Page> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).IsRequired().HasMaxLength(255);
        builder.HasIndex(e => e.Slug).IsUnique();

        builder.Property(e => e.Content).HasColumnType("nvarchar(max)");
        builder.Property(e => e.Status)
            .IsRequired()
            .HasDefaultValue(PublishStatus.Draft);
        builder.Property(e => e.PublishedAt);
    }
}