using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.WebApi
{
    public static class RecipeWebModule
    {
        public static IMvcBuilder AddRecipeWebModule(this IMvcBuilder builder, MediatRServiceConfiguration mediatrConfig)
        {
            builder.AddApplicationPart(typeof(RecipeWebModule).Assembly);
            builder.Services.AddDbContext<RecipeDbContext>();            

            return builder;
        }
    }
}
