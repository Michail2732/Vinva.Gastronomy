using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddSteps
{    
    public readonly record struct AddStepsCommand : IRequest<Result>
    {
        public required Guid RecipeId { get; init; }
        public RecipeStepDto[] Steps { get; init; }
    }
}