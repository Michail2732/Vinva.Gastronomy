using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Persistence.Models
{
    public sealed class RecipeIngredientsSetRecord
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PortionsQuantitiesJson { get; set; } = "[]";
        public PortionsCalculationType PortionsCalculateType { get; set; }
        public List<RecipeIngredientRecord> Ingredients { get; set; } = new();
    }
}