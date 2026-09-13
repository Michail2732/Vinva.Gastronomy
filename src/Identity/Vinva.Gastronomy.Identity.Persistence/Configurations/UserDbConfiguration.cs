using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common.Entities;
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
                .IsRequired()
                .Metadata.SetValueComparer(new ValueComparer<UserRole[]>(
                    (left, right) => ReferenceEquals(left, right) ||
                        (left != null && right != null && left.SequenceEqual(right)),
                    value => value == null
                        ? 0
                        : value.Aggregate(0, (hash, role) => HashCode.Combine(hash, role)),
                    value => value == null ? Array.Empty<UserRole>() : value.ToArray()));

            builder.Property(a => a.State)
                .IsRequired();

            builder.HasOne(a => a.Session)
                   .WithOne()
                   .HasForeignKey<UserSession>(a => a.UserId)
                     .OnDelete(DeleteBehavior.NoAction);

                 builder.HasOne(a => a.Registration)
                     .WithOne()
                     .HasPrincipalKey<User>(a => a.Email)
                     .HasForeignKey<UserRegistration>(a => a.Email)
                     .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
