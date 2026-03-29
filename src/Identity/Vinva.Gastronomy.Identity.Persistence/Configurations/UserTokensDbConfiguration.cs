using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Persistence.Configurations
{

    public class UserTokensDbConfiguration : IEntityTypeConfiguration<UserTokens>
    {
        public void Configure(EntityTypeBuilder<UserTokens> builder)
        {
            builder.ToTable("UserTokens");

            builder.HasKey(a => a.UserId);

            builder.Property(a => a.AccessToken)
                .IsRequired();

            builder.Property(a => a.RefreshToken)
                .IsRequired();

            builder.Property(a => a.ExpiresAt)
                .IsRequired();

            builder.HasIndex(a => a.AccessToken)
                .IsUnique();

            builder.HasIndex(a => a.RefreshToken)
                .IsUnique();
        }
    }
}
