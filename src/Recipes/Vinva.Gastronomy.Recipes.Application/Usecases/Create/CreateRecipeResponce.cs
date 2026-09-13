namespace Vinva.Gastronomy.Recipes.Application.Usecases.Create
{
    public sealed record CreateRecipeResponce
    {
        public required Guid RecipeId { get; init; }
        public required string Name { get; init; }
    }
}
