using MediatR;
using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Update
{    
    public readonly record struct UpdateCommand : IRequest
    {
        public required Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }        
        public TimeSpan? CookingTime { get; init; }
        public Guid? BaseRecipe { get; init; }                
        public RecipeIngredientDto[]? Ingredients { get; init; }
        public RecipePropertyDto[]? Properties { get; init; }
    }
}