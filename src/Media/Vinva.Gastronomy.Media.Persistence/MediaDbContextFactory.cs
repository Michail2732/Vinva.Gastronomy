using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Vinva.Gastronomy.Media.Persistence;

namespace Vinva.Gastronomy.Media.Infrastructure
{
    public class MediaDbContextFactory : IDesignTimeDbContextFactory<MediaDbContext>
    {
        private const string _debugConnectionString = "Host=localhost;Port=5432;Database=Gastronomy;Username=postgres;Password=postgres;Include Error Detail=true";

        public MediaDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<MediaDbContext> optionsBuilder = new();

            optionsBuilder.UseNpgsql(_debugConnectionString, e => e.MigrationsAssembly(GetType().Assembly.FullName)
                                                                    .MigrationsHistoryTable("_EFMigrationsHistory"))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();

            return new MediaDbContext(optionsBuilder.Options);
        }
    }
}
