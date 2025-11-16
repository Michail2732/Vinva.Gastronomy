using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Vinva.Gastronomy.Recipes.Infrastructure.Data.EntityFramework
{    
    public class RecipeDbContextFactory : IDesignTimeDbContextFactory<RecipeDbContext>
    {
        private const string _debugConnectionString = "Host=localhost;Port=5432;Database=Gastronomy;Username=postgres;Password=postgres;Include Error Detail=true";

        public RecipeDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<RecipeDbContext> optionsBuilder = new();

            optionsBuilder.UseNpgsql(_debugConnectionString, e => e.MigrationsAssembly(this.GetType().Assembly.FullName)
                                                                    .MigrationsHistoryTable("_EFMigrationsHistory"))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();

            return new RecipeDbContext(optionsBuilder.Options);
        }
    }
}
