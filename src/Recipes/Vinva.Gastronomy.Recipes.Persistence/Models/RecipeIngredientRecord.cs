using System;

namespace Vinva.Gastronomy.Recipes.Persistence.Models
{
    public sealed class RecipeIngredientRecord
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public Guid IngredientsSetId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string QuantitiesJson { get; set; } = "[]";
    }
}
