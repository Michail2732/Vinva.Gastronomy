using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Persistence.Converters;

namespace Vinva.Gastronomy.Identity.Persistence.Configurations
{
    public class UserDbConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

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

            builder.Property(a => a.Roles)
                .HasConversion<UserRoleArrayToStringConverter>()
                .IsRequired();

            builder.Property(a => a.State)
                .IsRequired();

            builder.HasOne(a => a.Tokens)
                   .WithOne()
                   .HasForeignKey<UserTokens>(a => a.UserId)
                   .OnDelete(DeleteBehavior.NoAction);
                
        }
    }
}
