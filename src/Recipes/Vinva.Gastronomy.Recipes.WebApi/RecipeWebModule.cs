using FluentValidation;
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

        public Task InitializeAsync(WebApplication webApp, CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }

        public void RegisterServices(WebModuleContext context)
        {
            var services = context.Services;

            context.ConfigureMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblies(typeof(Application.Common.CategoryDto).Assembly);
            });
            context.AddApplicationPart(GetType().Assembly);
            services.AddValidatorsFromAssembly(typeof(Application.Common.CategoryDto).Assembly);
            services.AddDbContext<RecipeDbContext>(options =>
            {
                options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnectionString"));
            });
        }
    }
}   
