using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.CategoryUsecases.RemoveCategory
{
    // Include properties to be used as input for the command
    public readonly record struct RemoveCategoryCommand : IRequest<Result>
    {
        public Guid[] CategoryIds { get; init; }
    }
}