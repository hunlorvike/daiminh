using core.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities
{
    public class Tag : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<ContentTag> ContentTags { get; set; } = new List<ContentTag>();
        public virtual ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
    }

    public class TagConfiguration : BaseEntityConfiguration<Tag, int>
    {
        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            base.Configure(builder);
            builder.ToTable("tags");

            builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
            builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(50);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").IsRequired();

            builder.HasIndex(e => e.Slug).HasDatabaseName("idx_tags_slug");
        }
    }
}