using MediatR;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Update
{    
    public readonly record struct UpdateIngredientCommand : IRequest
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? Comment { get; init; }
        public string? UsageComment { get; init; }
        public Guid? PhotoId { get; init; }
        public Guid? RecipeId { get; init; }
    }
}