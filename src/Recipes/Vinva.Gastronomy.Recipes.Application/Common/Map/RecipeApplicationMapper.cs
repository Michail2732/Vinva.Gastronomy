using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Domain.Models;

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
                RecipeId = ingredient.RecipeId                
            };
        }        

        public RecipeIngredient Map(RecipeIngredientDto ingredientDto, Guid recipeId)
        {
            return new RecipeIngredient(recipeId, ingredientDto.IngredientId
                , ingredientDto.IngredientName
                , new IngredientQuantities(ingredientDto.Quantities.Select(a =>
                     new IngredientQuantity
                     {
                         Measure = a.Measure,
                         Quantity = a.Quantity
                     }))
                , ingredientDto.IsRequired);            
        }

        public RecipeProperties Map(IEnumerable<RecipePropertyDto> propDtos)
        {

            return new RecipeProperties(propDtos.Select(a => new RecipeProperty(a.Name, a.Values)));
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
                CookingTime = recipe.CookingTime,
                TitleImageId = recipe.TitleImageId,
                OtherImageIds = recipe.OtherImageIds,
                VideoId = recipe.VideoId,
                Document = recipe.Document,
                Properties = recipe.Properties.Select(a => new RecipePropertyDto
                {
                    Name = a.Name,
                    Values = a.Values
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
                }).ToArray()                
            };
        }
    }
}
