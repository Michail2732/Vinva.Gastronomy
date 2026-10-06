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

        public IReadOnlyCollection<Type> DependsOn => Array.Empty<Type>();

        public async Task InitializeAsync(ModuleInitializationContext context, CancellationToken ct = default)
        {
            using (var scope = context.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<RecipeDbContext>()
                    ?? throw new ArgumentNullException($"Not found {nameof(RecipeDbContext)}");
                await dbContext.Database.MigrateAsync(ct);                
            }            
        }

        public void RegisterServices(ModuleServiceContext context)
        {
            var services = context.Services;

            services.AddValidatorsFromAssembly(typeof(Application.Common.CharacteristicDto).Assembly);
            services.AddDbContext<RecipeDbContext>(options =>
            {
                options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnectionString"));
            });
        }

        public void RegisterWeb(ModuleWebContext context)
        {
            context.ConfigureMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblies(typeof(Application.Common.CharacteristicDto).Assembly);
            });
            context.AddApplicationPart(GetType().Assembly);
        }
    }
}   
