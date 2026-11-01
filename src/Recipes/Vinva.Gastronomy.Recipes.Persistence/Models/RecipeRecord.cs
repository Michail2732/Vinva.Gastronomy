namespace Vinva.Gastronomy.Recipes.Persistence.Models
{
    public class RecipeRecord
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? BaseRecipe { get; set; }
        public string Document { get; set; } = string.Empty;
        public Guid? TitleImageId { get; set; }
        public Guid[]? GalleryImageIds { get; set; }
        public List<CharacteristicRecord> Characteristics { get; set; } = new();
        public List<RecipeIngredientsSetRecord> IngredientsSets { get; set; } = new();
    }
}