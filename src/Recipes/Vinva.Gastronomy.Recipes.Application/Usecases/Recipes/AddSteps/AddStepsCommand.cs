using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddSteps
{    
    public readonly record struct AddStepsCommand : IRequest
    {
        public required Guid RecipeId { get; init; }
        public RecipeStepDto[] Steps { get; init; }
    }
}