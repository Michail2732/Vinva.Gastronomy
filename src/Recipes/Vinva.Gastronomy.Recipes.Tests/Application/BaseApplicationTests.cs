using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Tests.ApplicationTests
{
    public abstract class BaseApplicationTests
    {
        protected RecipeDbContext DbContext { get; private set; } = null!;
        protected IConfiguration Configuration { get; private set; } = null!;
        protected ServiceProvider ServiceProvider { get; private set; } = null!;

        [SetUp]
        public void SetupApplicationTests()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();

            // Configure DbContext
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<RecipeDbContext>(options =>
                options.UseNpgsql(connectionString,
                    e => e.MigrationsAssembly(typeof(RecipeDbContext).Assembly.FullName)
                          .MigrationsHistoryTable("_EFMigrationsHistory"))
                      .LogTo(Console.WriteLine, LogLevel.Information)
                      .EnableSensitiveDataLogging());

            ServiceProvider = services.BuildServiceProvider();
            DbContext = ServiceProvider.GetRequiredService<RecipeDbContext>();

            // Clean and seed database
            var clearSqlRaw = File.ReadAllText("CleanupDb.sql");
            DbContext.Database.ExecuteSqlRaw(clearSqlRaw);

            var dumpSqlRaw = File.ReadAllText("TestDbDump.sql");
            DbContext.Database.EnsureCreated();
            DbContext.Database.ExecuteSqlRaw(dumpSqlRaw);
        }

        [TearDown]
        public void TearDownApplicationTests()
        {
            var clearSqlRaw = File.ReadAllText("CleanupDb.sql");
            DbContext.Database.ExecuteSqlRaw(clearSqlRaw);
            DbContext.Dispose();
            ServiceProvider.Dispose();
        }
    }
}
