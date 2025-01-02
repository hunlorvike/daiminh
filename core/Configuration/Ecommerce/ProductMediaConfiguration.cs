using core.Entities.Ecommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Configuration.Ecommerce;

public class ProductMediaConfiguration : IEntityTypeConfiguration<ProductMedia>
{
    public void Configure(EntityTypeBuilder<ProductMedia> builder)
    {
        builder.ToTable("ProductMedias");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MediaType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.MediaUrl)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Medias)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}