using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes
{
    public enum RecipeIncludes
    {
        None = 0,
        Ingredients = 1,
        Steps = 2,
        Categories = 4
    }

    public class BaseRecipeHandler
    {

        protected async Task<Recipe> GetRecipeById(RecipeDbContext dbContext, Guid id, RecipeIncludes includes = default, CancellationToken ct = default)
        {
            var recipeQuery = dbContext.Recipes.AsQueryable();
            if (includes.HasFlag(RecipeIncludes.Ingredients))
            {
                recipeQuery = recipeQuery.Include(a => a.Ingredients);
            }
            if (includes.HasFlag(RecipeIncludes.Steps))
            {
                recipeQuery = recipeQuery.Include(a => a.Steps);
            }
            if (includes.HasFlag(RecipeIncludes.Categories))
            {
                recipeQuery = recipeQuery.Include(a => a.Categories);
            }

            var recipe = await recipeQuery.FirstOrDefaultAsync(a => a.Id == id, ct);

            if (recipe == null)
                throw new NotFoundException(RecipesApplicationErrors.RecipeNotFound);

            return recipe;
        }

    }
}
