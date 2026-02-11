using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveRecipeCategory
{
    // Include properties to be used as input for the command
    public readonly record struct RemoveRecipeCategoryCommand : IRequest<Result>
    {
        public Guid RecipeId { get; init; }
        public Guid[] CategoryIds { get; init; }
    }
}