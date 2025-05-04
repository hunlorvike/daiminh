using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class Slide : BaseEntity<int>
{
    public string Title { get; set; } = string.Empty;             // tiêu đề chính
    public string? Subtitle { get; set; }                         // tiêu đề phụ
    public string? Description { get; set; }                      // mô tả ngắn gọn
    public string ImageUrl { get; set; } = string.Empty;          // ảnh nền slide
    public string? CtaText { get; set; }                          // text nút CTA (ví dụ: Xem ngay)
    public string? CtaLink { get; set; }                          // liên kết CTA
    public string? Target { get; set; } = "_self";                // _blank nếu mở tab mới
    public int OrderIndex { get; set; } = 0;                      // thứ tự hiển thị
    public bool IsActive { get; set; } = true;                    // slide đang hoạt động
    public DateTime? StartAt { get; set; }                        // thời gian bắt đầu hiển thị
    public DateTime? EndAt { get; set; }                          // thời gian kết thúc
}

public class SlideConfiguration : BaseEntityConfiguration<Slide, int>
{
    public override void Configure(EntityTypeBuilder<Slide> builder)
    {
        base.Configure(builder);
        builder.ToTable("slides");

        builder.Property(e => e.Title).HasColumnName("title").IsRequired().HasMaxLength(150);
        builder.Property(e => e.Subtitle).HasColumnName("subtitle").HasMaxLength(150);
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
        builder.Property(e => e.ImageUrl).HasColumnName("image_url").IsRequired().HasMaxLength(255);
        builder.Property(e => e.CtaText).HasColumnName("cta_text").HasMaxLength(100);
        builder.Property(e => e.CtaLink).HasColumnName("cta_link").HasMaxLength(255);
        builder.Property(e => e.Target).HasColumnName("target").HasMaxLength(10).HasDefaultValue("_self");

        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.StartAt).HasColumnName("start_at");
        builder.Property(e => e.EndAt).HasColumnName("end_at");

        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.OrderIndex);
    }
}
