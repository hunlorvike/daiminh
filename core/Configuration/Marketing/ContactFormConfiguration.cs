using core.Common.Enums;
using core.Entities.Marketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Configuration.Marketing;

public class ContactFormConfiguration : IEntityTypeConfiguration<ContactForm>
{
    public void Configure(EntityTypeBuilder<ContactForm> builder)
    {
        builder.ToTable("ContactForms");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(256);

        builder.Property(x => x.Phone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Subject)
            .HasMaxLength(200);

        builder.Property(x => x.Status)
            .HasMaxLength(20)
            .HasDefaultValue(ContactFormStatus.New);

        builder.Property(x => x.IPAddress)
            .HasMaxLength(50);

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_ContactForms_Status");

        builder.HasIndex(x => x.Email)
            .HasDatabaseName("IX_ContactForms_Email");

        builder.HasIndex(x => x.Phone)
            .HasDatabaseName("IX_ContactForms_Phone");
    }
}