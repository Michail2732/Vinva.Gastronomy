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
                RecipeId = ingredient.RecipeId,
                Name = ingredient.Name,
                Part = ingredient.Part,
                QuantitiesJson = JsonSerializer.Serialize(ingredient.Quantities),
            };
        }

        public static RecipeIngredient ToDomain(RecipeIngredientRecord record)
        {
            var quantities = JsonSerializer.Deserialize<List<RecipeIngredientQuantity>>(record.QuantitiesJson)
                ?? new List<RecipeIngredientQuantity>();
            return new RecipeIngredient(record.RecipeId, record.Name, quantities, record.Part);
        }
    }
}
