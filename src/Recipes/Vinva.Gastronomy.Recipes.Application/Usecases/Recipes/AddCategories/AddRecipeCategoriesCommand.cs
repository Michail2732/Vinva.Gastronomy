using MediatR;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddCategories
{    
    public readonly record struct AddRecipeCategoriesCommand : IRequest
    {
        public Guid RecipeId { get; init; }
        public Guid[] CategoryIds { get; init; }
    }
}