using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddRecipeCategories
{    
    public readonly record struct AddRecipeCategoriesCommand : IRequest<Result>
    {
        public Guid RecipeId { get; init; }
        public Guid[] CategoryIds { get; init; }
    }
}