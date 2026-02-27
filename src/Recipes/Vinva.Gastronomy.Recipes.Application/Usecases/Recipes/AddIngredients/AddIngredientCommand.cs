using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddIngredients
{    
    public readonly record struct AddIngredientCommand : IRequest
    {
        public Guid RecipeId { get; init; }
        public RecipeIngredientDto[] Ingredients { get; init; }
    }
}