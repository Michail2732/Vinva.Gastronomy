using MediatR;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveCategory
{    
    public readonly record struct RemoveRecipeCategoryCommand : IRequest
    {
        public Guid RecipeId { get; init; }
        public Guid[] CategoryIds { get; init; }
    }
}