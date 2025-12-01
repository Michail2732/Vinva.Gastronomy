using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Tests.Data.Repositories
{
    public abstract class BaseRepositoryTests
    {
        protected readonly IConfiguration _configuration;
        protected readonly DbContextOptions<RecipeDbContext> _dbContextOptions;
        protected RecipeDbContext _recipeDbContext;        


        public BaseRepositoryTests()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            DbContextOptionsBuilder<RecipeDbContext> optionsBuilder = new();
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"),
                                     e => e.MigrationsAssembly(this.GetType().Assembly.FullName)
                          .MigrationsHistoryTable("_EFMigrationsHistory"))
                          .LogTo(Console.WriteLine, LogLevel.Information)
                          .EnableSensitiveDataLogging();
            _dbContextOptions = optionsBuilder.Options;
            _recipeDbContext = null!;            
        }


        [SetUp]
        public void SetupRepository()
        {
            _recipeDbContext = new RecipeDbContext(_dbContextOptions);            

            var clearSqlRaw = File.ReadAllText("CleanupDb.sql");
            _recipeDbContext.Database.ExecuteSqlRaw(clearSqlRaw);

            var dumpSqlRaw = File.ReadAllText("TestDbDump.sql");
            _recipeDbContext.Database.EnsureCreated();
            _recipeDbContext.Database.ExecuteSqlRaw(dumpSqlRaw);
        }

        [TearDown]
        public void ClearRepository()
        {
            var clearSqlRaw = File.ReadAllText("CleanupDb.sql");
            _recipeDbContext.Database.ExecuteSqlRaw(clearSqlRaw);
            _recipeDbContext.Dispose();
        }
    }
}
