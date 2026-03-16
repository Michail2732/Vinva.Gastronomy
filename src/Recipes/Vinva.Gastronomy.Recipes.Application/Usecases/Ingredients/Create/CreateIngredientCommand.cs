using MediatR;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Create
{    
    public readonly record struct CreateIngredientCommand : IRequest<CreateIngredientResponse>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string? UsageComment { get; init; }        
    }
}
