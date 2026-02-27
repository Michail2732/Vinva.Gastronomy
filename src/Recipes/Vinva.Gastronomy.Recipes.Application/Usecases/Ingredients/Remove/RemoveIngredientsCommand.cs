using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Remove
{    
    public readonly record struct RemoveIngredientsCommand : IRequest
    {
        public Guid[] IngredientIds { get; init; }
    }
}