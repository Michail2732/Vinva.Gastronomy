using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Persistence.Configurations;

namespace Vinva.Gastronomy.Identity.Persistence
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<User> Users { get; private init; }
        public DbSet<UserTokens> UserTokens { get; private init; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserDbConfiguration());
            modelBuilder.ApplyConfiguration(new UserTokensDbConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
