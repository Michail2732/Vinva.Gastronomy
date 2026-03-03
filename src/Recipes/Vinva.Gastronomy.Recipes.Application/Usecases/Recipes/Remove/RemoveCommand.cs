using MediatR;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Remove
{    
    public readonly record struct RemoveCommand : IRequest
    {
        public Guid RecipeId { get; init; }
    }
}