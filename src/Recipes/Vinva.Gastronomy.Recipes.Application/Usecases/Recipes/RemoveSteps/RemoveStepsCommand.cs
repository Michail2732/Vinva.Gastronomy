using MediatR;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveSteps
{    
    public readonly record struct RemoveStepsCommand : IRequest
    {
        public required Guid RecipeId { get; init; }
        public required int[] SeqNumbers { get; init; }
    }
}