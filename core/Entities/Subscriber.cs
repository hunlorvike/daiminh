using core.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class Subscriber : BaseEntity<int>
{
    public string Email { get; set; } = string.Empty;
    public SubscriberStatus Status { get; set; } = SubscriberStatus.Pending;
}

public class SubscriberConfiguration : BaseEntityConfiguration<Subscriber, int>
{
    public override void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        base.Configure(builder);

        builder.ToTable("subscriber");

        builder.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();
        builder.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<SubscriberStatus>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(SubscriberStatus.Pending);


        builder.HasIndex(e => e.Email).HasDatabaseName("idx_subscriber_email");
    }
}