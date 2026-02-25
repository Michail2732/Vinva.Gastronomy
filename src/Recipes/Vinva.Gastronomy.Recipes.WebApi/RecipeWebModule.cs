using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        public Assembly[] Assemblies { get; } =
        {
            typeof(Domain.Entities.Recipe).Assembly,
            typeof(Application.Common.CategoryDto).Assembly,
            typeof(Persistence.RecipeDbContext).Assembly,
        };

        public Task InitializeAsync(WebApplication webApp, CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }

        public void RegisterServices(WebApplicationBuilder webAppBuilder, IConfiguration config)
        {
            var services = webAppBuilder.Services;
            services.AddDbContext<RecipeDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnectionString"));
            });
        }
    }
}
