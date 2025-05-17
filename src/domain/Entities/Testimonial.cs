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

        builder.HasData(
             new Testimonial { Id = 1, ClientName = "Nguyễn Văn A", ClientTitle = "Chủ nhà", Content = "Sơn của Đại Minh rất bền màu và dễ thi công. Tôi rất hài lòng!", Rating = 5, IsActive = true, OrderIndex = 0, CreatedAt = DateTime.UtcNow },
             new Testimonial { Id = 2, ClientName = "Trần Thị B", ClientTitle = "Nhà thầu", ClientCompany = "Công ty Xây dựng B&B", Content = "Vật liệu chống thấm Sika từ Đại Minh luôn đảm bảo chất lượng cho công trình của chúng tôi.", Rating = 5, IsActive = true, OrderIndex = 1, CreatedAt = DateTime.UtcNow },
             new Testimonial { Id = 3, ClientName = "Lê Văn C", ClientTitle = "Khách hàng cá nhân", Content = "Được tư vấn rất nhiệt tình để chọn đúng loại sơn cho ngôi nhà cũ. Dịch vụ tuyệt vời!", Rating = 4, IsActive = true, OrderIndex = 2, CreatedAt = DateTime.UtcNow }
         );
    }
}