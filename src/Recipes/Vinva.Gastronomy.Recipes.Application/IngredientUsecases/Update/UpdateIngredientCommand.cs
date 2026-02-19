using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.IngredientUsecases.Update
{
    
    public record UpdateIngredientCommand : IRequest<Result>
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? Comment { get; init; }
        public string? UsageComment { get; init; }
        public Guid? PhotoId { get; init; }
        public Guid? RecipeId { get; init; }
    }
}