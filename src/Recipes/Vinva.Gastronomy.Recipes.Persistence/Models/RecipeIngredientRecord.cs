using System;

namespace Vinva.Gastronomy.Recipes.Persistence.Models
{
    public sealed class RecipeIngredientRecord
    {
        public Guid RecipeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Part { get; set; }
        public string QuantitiesJson { get; set; } = "[]";
    }
}
