using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Persistence.Configurations
{
    public class RegistrationTokensDbConfiguration : IEntityTypeConfiguration<RegistrationToken>
    {
        public void Configure(EntityTypeBuilder<RegistrationToken> builder)
        {
            builder.ToTable("RegistrationTokens");

            builder.HasKey(a => a.Id);

            builder.HasIndex(a => a.Login)
                .IsUnique();

            builder.HasIndex(a => a.Email)
                .IsUnique();

            builder.Property(a => a.Login)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.ExpiresAt)
                .IsRequired();

            builder.Property(a => a.PasswordHash)
                .IsRequired();
        }
    }
}
