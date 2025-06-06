using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace domain.Entities.Shared;

public abstract class BaseEntity<TKey> where TKey : IEquatable<TKey>
{
    [Key]
    public TKey Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}

public abstract class BaseEntityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .HasColumnType("timestamp without time zone");

        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired(false);
    }
}