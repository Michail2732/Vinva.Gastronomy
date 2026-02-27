namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Create
{
    public readonly record struct CreateIngredientResponse 
    {
        public Guid IngredientId { get; init; }

    }
}
