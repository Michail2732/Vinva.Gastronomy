using MediatR;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Remove
{    
    public readonly record struct RemoveIngredientCommand : IRequest
    {
        public Guid IngredientId { get; init; }
    }
}