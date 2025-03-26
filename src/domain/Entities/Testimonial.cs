using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class Testimonial : BaseEntity<int>
{
    public string ClientName { get; set; } = string.Empty;
    public string? ClientTitle { get; set; }
    public string? ClientCompany { get; set; }
    public string? ClientAvatar { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } = 5; // 1-5 stars
    public bool IsActive { get; set; } = true;
    public int OrderIndex { get; set; } = 0;
    public string? ProjectReference { get; set; } // Có thể là tên dự án hoặc ID
}

public class TestimonialConfiguration : BaseEntityConfiguration<Testimonial, int>
{
    public override void Configure(EntityTypeBuilder<Testimonial> builder)
    {
        base.Configure(builder);

        builder.ToTable("testimonials");

        builder.Property(e => e.ClientName).HasColumnName("client_name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.ClientTitle).HasColumnName("client_title").HasMaxLength(100);
        builder.Property(e => e.ClientCompany).HasColumnName("client_company").HasMaxLength(100);
        builder.Property(e => e.ClientAvatar).HasColumnName("client_avatar").HasMaxLength(255);
        builder.Property(e => e.Content).HasColumnName("content").IsRequired().HasColumnType("text");
        builder.Property(e => e.Rating).HasColumnName("rating").HasDefaultValue(5);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);
        builder.Property(e => e.ProjectReference).HasColumnName("project_reference").HasMaxLength(255);

        builder.HasIndex(e => e.IsActive).HasDatabaseName("idx_testimonials_is_active");
        builder.HasIndex(e => e.OrderIndex).HasDatabaseName("idx_testimonials_order_index");
        builder.HasIndex(e => e.Rating).HasDatabaseName("idx_testimonials_rating");
    }
}

