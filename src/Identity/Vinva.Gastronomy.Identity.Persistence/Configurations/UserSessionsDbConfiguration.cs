using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Persistence.Configurations
{

    public class UserSessionsDbConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.ToTable("UserSessions");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.UserId)
                .IsRequired();

            builder.HasIndex(a => a.UserId)
                .IsUnique();

            builder.Property(a => a.DeviceId)
                .HasMaxLength(100);

            builder.Property(a => a.ExpiresAt)
                .IsRequired();

            builder.Property(a => a.RevokedAt);

            builder.Property(a => a.RefreshTokenHash)
                .IsRequired();

            builder.HasIndex(a => a.RefreshTokenHash)
                .IsUnique();
        }
    }
}
