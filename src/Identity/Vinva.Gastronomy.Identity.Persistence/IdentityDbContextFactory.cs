using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Vinva.Gastronomy.Identity.Persistence;

namespace Vinva.Gastronomy.Identity.Infrastructure
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        private const string _debugConnectionString = "Host=localhost;Port=5432;Database=Gastronomy;Username=postgres;Password=postgres;Include Error Detail=true";

        public IdentityDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<IdentityDbContext> optionsBuilder = new();

            optionsBuilder.UseNpgsql(_debugConnectionString, e => e.MigrationsAssembly(GetType().Assembly.FullName)
                                                                    .MigrationsHistoryTable("_EFMigrationsHistory"))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();

            return new IdentityDbContext(optionsBuilder.Options);
        }
    }
}
