using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Modularity;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.WebApi
{
    public class RecipeWebModule : IWebModule
    {
        public string ModuleName => "Recipe";

        public int Order => 0;        

        public async Task InitializeAsync(WebApplication webApp, CancellationToken ct = default)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<RecipeDbContext>()
                    ?? throw new ArgumentNullException($"Not found {nameof(RecipeDbContext)}");
                await context!.Database.MigrateAsync();
                if (webApp.Environment.IsDevelopment())
                {
                    var recipeCount = context.Recipes.Count();
                    var ingredientsCount = context.Ingredients.Count();
                    if (recipeCount == 0 && ingredientsCount == 0)
                    {
                        var baseDir = AppDomain.CurrentDomain.BaseDirectory;                        
                        var recipesGeneratorSqlPath = Path.Combine(baseDir, "Data/RecipesGenerator.sql");
                        var ingredientsGeneratorSqlPath = Path.Combine(baseDir, "./Data/IngredientsGenerator.sql");
                        if (File.Exists(recipesGeneratorSqlPath))
                        {
                            var recipesGeneratorSql = await File.ReadAllTextAsync(recipesGeneratorSqlPath, ct);
                            await context.Database.ExecuteSqlRawAsync(recipesGeneratorSql, ct);
                        }                            
                        if (File.Exists(ingredientsGeneratorSqlPath))
                        {
                            var ingredientsGeneratorSql = await File.ReadAllTextAsync(ingredientsGeneratorSqlPath, ct);
                            await context.Database.ExecuteSqlRawAsync(ingredientsGeneratorSql, ct);
                        }                        
                    }
                }
            }            
        }

        public void RegisterServices(WebModuleContext context)
        {
            var services = context.Services;

            context.ConfigureMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblies(typeof(Application.Common.IngredientDto).Assembly);
            });
            context.AddApplicationPart(GetType().Assembly);
            services.AddValidatorsFromAssembly(typeof(Application.Common.IngredientDto).Assembly);
            services.AddDbContext<RecipeDbContext>(options =>
            {
                options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnectionString"));
            });
        }
    }
}   
