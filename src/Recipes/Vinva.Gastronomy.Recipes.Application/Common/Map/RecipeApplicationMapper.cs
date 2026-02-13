using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Application.Common.Map
{
    public class RecipeApplicationMapper
    {
        public IngredientDto Map(Ingredient ingredient)
        {
            return new IngredientDto
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Description = ingredient.Description,
                PhotoId = ingredient.PhotoId,
                RecipeId = ingredient.RecipeId,
                UsageComment = ingredient.UsageComment,
                Categories = ingredient.Categories?.Select(a =>
                    new IngredientCategoryDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Description = a.Description,
                    }).ToArray()
                    ?? Array.Empty<IngredientCategoryDto>()
            };
        }

        public List<RecipeDto> Map(IEnumerable<Recipe> recipes)
        {
            List<RecipeDto> result = new List<RecipeDto>();
            foreach (var recipe in recipes)
            {
                result.Add(Map(recipe));
            }
            return result;
        }

        public RecipeDto Map(Recipe recipe)
        {
            return new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                BaseRecipe = recipe.BaseRecipe,
                CookingComment = recipe.CookingComment,
                CookingTime = recipe.CookingTime,
                IngredientComment = recipe.IngredientComment,
                PhotoId = recipe.PhotoId,
                StorageComment = recipe.StorageComment,
                UsageComment = recipe.UsageComment,
                VideoId = recipe.VideoId,
                Categories = recipe.Categories.Select(a => new RecipeCategoryDto
                {
                    Id = a.Id,
                    Name = a.Name,
                }).ToArray(),
                Ingredients = recipe.Ingredients.Select(a => new RecipeIngredientDto
                {
                    IngredientId = a.IngredientId,
                    IngredientName = a.Name,
                    IsRequired = a.IsRequired,
                    Quantities = a.Quantities.Select(a => new IngredientQuantityDto
                    {
                        Measure = a.Measure,
                        Quantity = a.Quantity
                    }).ToArray()
                }).ToArray(),
                Steps = recipe.Steps.Select(a => new RecipeStepDto
                {
                    Description = a.Description,
                    Name = a.Name,
                    Comment = a.Comment,
                    SeqNumber = a.SeqNumber,
                    PhotoId = a.PhotoId
                }).ToArray(),
            };
        }
    }
}
