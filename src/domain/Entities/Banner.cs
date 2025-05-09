using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;

public class Banner : BaseEntity<int>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? LinkUrl { get; set; }
    public BannerType Type { get; set; } = BannerType.Header;
    public bool IsActive { get; set; } = true;
    public int OrderIndex { get; set; } = 0;
}

public class BannerConfiguration : BaseEntityConfiguration<Banner, int>
{
    public override void Configure(EntityTypeBuilder<Banner> builder)
    {
        base.Configure(builder);

        builder.ToTable("banners");

        builder.Property(e => e.Title).HasColumnName("title").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Description).HasColumnName("description").HasColumnType("nvarchar(max)");
        builder.Property(e => e.ImageUrl).HasColumnName("image_url").IsRequired().HasMaxLength(2048);
        builder.Property(e => e.LinkUrl).HasColumnName("link_url").HasMaxLength(2048);
        builder.Property(e => e.Type).HasColumnName("type").IsRequired().HasDefaultValue(BannerType.Header);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);
    }
}