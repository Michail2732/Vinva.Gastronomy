namespace Vinva.Gastronomy.Recipes.Application.IngredientUsecases.CreateIngredient
{
    public readonly record struct CreateIngredientResponse 
    {
        public Guid IngredientId { get; init; }

    }
}
