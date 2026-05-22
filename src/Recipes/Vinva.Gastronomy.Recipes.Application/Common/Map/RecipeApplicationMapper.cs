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

        public RecipeStep Map(RecipeStepDto stepDto, Guid recipeId)
        {
            return new RecipeStep(stepDto.Id, recipeId, stepDto.Description, stepDto.SeqNumber, stepDto.PhotoId)
            {
                
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
                , ingredientDto.IsRequired
                , ingredientDto.Comment);            
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
                Comment = recipe.Comment,
                BaseRecipe = recipe.BaseRecipe,
                CookingComment = recipe.CookingComment,
                CookingTime = recipe.CookingTime,
                IngredientComment = recipe.IngredientComment,
                TitleImageId = recipe.TitleImageId,
                OtherImageIds = recipe.OtherImageIds,
                StorageComment = recipe.StorageComment,
                UsageComment = recipe.UsageComment,
                VideoId = recipe.VideoId,
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
                }).ToArray(),
                Steps = recipe.Steps.Select(a => new RecipeStepDto
                {                    
                    Id = a.Id,
                    Description = a.Description,                    
                    Comment = a.Comment,
                    SeqNumber = a.SeqNumber,
                    PhotoId = a.PhotoId
                }).ToArray(),
            };
        }
    }
}
