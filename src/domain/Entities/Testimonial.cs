using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class Testimonial : BaseEntity<int>
{
    public string ClientName { get; set; } = string.Empty;
    public string? ClientTitle { get; set; }
    public string? ClientCompany { get; set; }
    public string? ClientAvatar { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public bool IsActive { get; set; } = true;
    public int OrderIndex { get; set; } = 0;
}

public class TestimonialConfiguration : BaseEntityConfiguration<Testimonial, int>
{
    public override void Configure(EntityTypeBuilder<Testimonial> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.ClientName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.ClientTitle).HasMaxLength(100);
        builder.Property(e => e.ClientCompany).HasMaxLength(100);
        builder.Property(e => e.ClientAvatar).HasMaxLength(255);
        builder.Property(e => e.Content).IsRequired().HasColumnType("nvarchar(max)");
        builder.Property(e => e.Rating).HasDefaultValue(5);

        builder.HasIndex(e => e.Rating);
        builder.HasIndex(e => e.OrderIndex);

        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.OrderIndex).HasDefaultValue(0);
    }
}