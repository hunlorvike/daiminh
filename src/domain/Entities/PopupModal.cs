using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class PopupModal : BaseEntity<int>
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public string? LinkUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class PopupModalConfiguration : BaseEntityConfiguration<PopupModal, int>
{
    public override void Configure(EntityTypeBuilder<PopupModal> builder)
    {
        base.Configure(builder);

        builder.ToTable("popup_modals");

        builder.Property(e => e.Title).HasColumnName("title").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Content).HasColumnName("content").HasColumnType("nvarchar(max)");
        builder.Property(e => e.ImageUrl).HasColumnName("image_url").HasMaxLength(2048);
        builder.Property(e => e.LinkUrl).HasColumnName("link_url").HasMaxLength(2048);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.StartDate).HasColumnName("start_date");
        builder.Property(e => e.EndDate).HasColumnName("end_date");
    }
}