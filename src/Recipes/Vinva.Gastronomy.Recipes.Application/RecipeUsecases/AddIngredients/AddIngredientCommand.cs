using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddIngredients
{    
    public record AddIngredientCommand : IRequest<Result>
    {
        public Guid RecipeId { get; init; }
        public RecipeIngredientDto[] Ingredients { get; init; } = [];
    }
}