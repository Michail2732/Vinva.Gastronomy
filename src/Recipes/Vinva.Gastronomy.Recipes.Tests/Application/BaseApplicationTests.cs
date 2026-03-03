using System;
using System.IO;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vinva.Gastronomy.Common.Modularity.MediatR;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Tests.Application
{
    public abstract class BaseApplicationTests
    {
        protected RecipeDbContext DbContext { get; private set; } = null!;
        protected IConfiguration Configuration { get; private set; } = null!;
        protected ServiceProvider ServiceProvider { get; private set; } = null!;
        protected IMediator Mediator { get; private set; } = null!;

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

            services.AddOptions(); 
            services.AddMemoryCache(); 
            services.AddHttpClient();
            services.AddSingleton(a => GuidProvider.Instance);

            services.AddMediatR(cfg =>
            {                
                cfg.RegisterServicesFromAssembly(typeof(BaseRecipeHandler).Assembly);
                cfg.AddOpenBehavior(typeof(LoggingMediatRBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationMediatRBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(BaseRecipeHandler).Assembly);

            ServiceProvider = services.BuildServiceProvider();
            DbContext = ServiceProvider.GetRequiredService<RecipeDbContext>();

            Mediator = ServiceProvider.GetRequiredService<IMediator>();

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
