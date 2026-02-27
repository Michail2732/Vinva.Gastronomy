namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Create
{
    public readonly record struct CreateRecipeResponce
    {
        public Guid RecipeId { get; init; }
        public string Name { get; init; }
    }
}
