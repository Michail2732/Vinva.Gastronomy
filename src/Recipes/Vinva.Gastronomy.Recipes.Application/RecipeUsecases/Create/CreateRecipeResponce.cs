namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.CreateRecipe
{
    public readonly record struct CreateRecipeResponce
    {
        public Guid RecipeId { get; init; }
        public string Name { get; init; }
    }
}
