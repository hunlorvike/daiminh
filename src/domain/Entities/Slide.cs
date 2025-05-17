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
        builder.Property(e => e.Title).IsRequired().HasMaxLength(150);
        builder.Property(e => e.Subtitle).HasMaxLength(150);
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.ImageUrl).IsRequired().HasMaxLength(255);
        builder.Property(e => e.CtaText).HasMaxLength(100);
        builder.Property(e => e.CtaLink).HasMaxLength(255);
        builder.Property(e => e.Target).HasMaxLength(10).HasDefaultValue("_self");

        builder.Property(e => e.OrderIndex).HasDefaultValue(0);
        builder.Property(e => e.IsActive).HasDefaultValue(true);

        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.OrderIndex);
    }
}
