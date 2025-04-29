using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class FAQ : BaseEntity<int>
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int OrderIndex { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
}

public class FAQConfiguration : BaseEntityConfiguration<FAQ, int>
{
    public override void Configure(EntityTypeBuilder<FAQ> builder)
    {
        base.Configure(builder);
        builder.ToTable("faqs");
        builder.Property(e => e.Question).HasColumnName("question").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Answer).HasColumnName("answer").IsRequired().HasColumnType("nvarchar(max)");
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);
        builder.HasIndex(e => e.OrderIndex);

        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.CategoryId).HasColumnName("category_id");
        builder.HasOne(e => e.Category)
            .WithMany(c => c.FAQs)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(
             new FAQ { Id = 1, CategoryId = 5, Question = "Sơn nội thất và sơn ngoại thất khác nhau như thế nào?", Answer = "Sơn nội thất và ngoại thất khác nhau về thành phần hóa học để phù hợp với điều kiện môi trường. Sơn ngoại thất chứa các chất chống tia UV, chống thấm tốt hơn để chịu được nắng, mưa, ẩm ướt. Sơn nội thất an toàn hơn cho sức khỏe, ít mùi và có độ bền màu trong nhà tốt.", OrderIndex = 0, IsActive = true, CreatedAt = DateTime.UtcNow },
             new FAQ { Id = 2, CategoryId = 5, Question = "Làm thế nào để tính toán lượng sơn cần dùng?", Answer = "Lượng sơn cần dùng phụ thuộc vào diện tích cần sơn, loại sơn, và bề mặt. Trung bình 1 lít sơn có thể phủ được 8-10m2 cho 2 lớp. Bạn cần đo diện tích tường, trần nhà và tham khảo hướng dẫn của nhà sản xuất sơn.", OrderIndex = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
             new FAQ { Id = 3, CategoryId = 5, Question = "Khi nào cần sử dụng sơn lót chống kiềm?", Answer = "Sơn lót chống kiềm cần được sử dụng trên các bề mặt mới xây (vữa, bê tông) hoặc các bề mặt cũ có dấu hiệu bị kiềm hóa (ố vàng, phấn trắng) để ngăn chặn kiềm từ xi măng ăn mòn lớp sơn phủ màu.", OrderIndex = 2, IsActive = true, CreatedAt = DateTime.UtcNow }
         );
    }
}