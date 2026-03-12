using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Media.Domain.Entities;
using Vinva.Gastronomy.Media.Persistence.Configurations;

namespace Vinva.Gastronomy.Media.Persistence
{
    public class MediaDbContext: DbContext
    {
        public DbSet<MediaItem> Files { get; init; }

        public MediaDbContext(DbContextOptions<MediaDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MediaFileDbConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
