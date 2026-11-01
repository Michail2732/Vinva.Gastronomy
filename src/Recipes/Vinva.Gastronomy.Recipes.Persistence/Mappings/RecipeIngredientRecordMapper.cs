using System.Text.Json;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence.Models;

namespace Vinva.Gastronomy.Recipes.Persistence.Mappings
{
    public static class RecipeIngredientRecordMapper
    {
        public static RecipeIngredientRecord ToRecord(RecipeIngredient ingredient)
        {
            return new RecipeIngredientRecord
            {
                Id = ingredient.Id,
                RecipeId = ingredient.RecipeId,
                IngredientsSetId = ingredient.IngredientsSetId,
                Name = ingredient.Name,
                QuantitiesJson = JsonSerializer.Serialize(ingredient.Quantities),
            };
        }

        public static RecipeIngredient ToDomain(RecipeIngredientRecord record)
        {
            var quantities = JsonSerializer.Deserialize<List<Quantity>>(record.QuantitiesJson)
                ?? new List<Quantity>();
            return new RecipeIngredient(record.Id, record.RecipeId, record.Name, record.IngredientsSetId, quantities);
        }
    }
}
