using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;
using shared.Models;

namespace domain.Entities;

public class Slider : BaseEntity<int>
{
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public string? LinkUrl { get; set; }
    public int Order { get; set; }
    public string? OverlayHtml { get; set; }
    public OverlayPosition? OverlayPosition { get; set; }
}

public class SliderConfiguration : BaseEntityConfiguration<Slider, int>
{
    public override void Configure(EntityTypeBuilder<Slider> builder)
    {
        base.Configure(builder);

        builder.ToTable("slider");

        builder.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();
        builder.Property(e => e.Title).HasColumnName("title").IsRequired();
        builder.Property(e => e.ImageUrl).HasColumnName("image_url").IsRequired();
        builder.Property(e => e.LinkUrl).HasColumnName("link_url");
        builder.Property(e => e.Order).HasColumnName("order_number").IsRequired();
        builder.Property(e => e.OverlayHtml).HasColumnName("overlay_html");
        builder.Property(e => e.OverlayPosition).HasColumnName("overlay_position")
            .HasConversion(
                v => v.ToString()!.ToLowerInvariant(),
                v => Enum.Parse<OverlayPosition>(v, true)
            );
    }
}