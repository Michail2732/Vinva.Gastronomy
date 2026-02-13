using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveIngredients
{
    
    public readonly record struct RemoveIngredientsCommand : IRequest<Result>
    {
        public Guid RecipeId { get; init; }
        public Guid[] IngredientIds { get; init; }
    }
}