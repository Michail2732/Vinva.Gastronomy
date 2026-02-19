using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.IngredientUsecases.RemoveIngredients
{
    // Include properties to be used as input for the command
    public readonly record struct RemoveIngredientsCommand : IRequest<Result>
    {
        public Guid[] IngredientIds { get; init; }
    }
}