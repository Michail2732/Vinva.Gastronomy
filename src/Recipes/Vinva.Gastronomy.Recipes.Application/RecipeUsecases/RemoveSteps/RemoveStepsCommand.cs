using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveSteps
{    
    public readonly record struct RemoveStepsCommand : IRequest<Result>
    {
        public required Guid RecipeId { get; init; }
        public required int[] SeqNumbers { get; init; }
    }
}