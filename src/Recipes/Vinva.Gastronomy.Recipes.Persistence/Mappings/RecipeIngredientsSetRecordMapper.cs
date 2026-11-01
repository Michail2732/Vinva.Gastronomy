using System.Text.Json;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence.Models;

namespace Vinva.Gastronomy.Recipes.Persistence.Mappings
{
    public static class RecipeIngredientsSetRecordMapper
    {
        public static RecipeIngredientsSetRecord ToRecord(RecipeIngredientsSet ingredientSet)
        {
            return new RecipeIngredientsSetRecord
            {
                Id = ingredientSet.Id,
                RecipeId = ingredientSet.RecipeId,
                Name = ingredientSet.Name,
                PortionsQuantitiesJson = JsonSerializer.Serialize(ingredientSet.PortionsQuantities),
                PortionsCalculateType = ingredientSet.PortionsCalculateType,
                Ingredients = ingredientSet.Ingredients.Select(RecipeIngredientRecordMapper.ToRecord).ToList()
            };
        }

        public static RecipeIngredientsSet ToDomain(RecipeIngredientsSetRecord record)
        {
            var quantities = JsonSerializer.Deserialize<List<Quantity>>(record.PortionsQuantitiesJson)
                ?? new List<Quantity>();
            var ingredients = record.Ingredients.Select(RecipeIngredientRecordMapper.ToDomain);
            return new RecipeIngredientsSet(record.Id, record.RecipeId, record.Name, quantities, ingredients)
            {
                PortionsCalculateType = record.PortionsCalculateType
            };
        }
    }
}