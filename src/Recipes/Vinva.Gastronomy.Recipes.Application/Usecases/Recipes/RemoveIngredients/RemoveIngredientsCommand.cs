using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveIngredients
{
    
    public readonly record struct RemoveIngredientsCommand : IRequest
    {
        public Guid RecipeId { get; init; }
        public Guid[] IngredientIds { get; init; }
    }
}