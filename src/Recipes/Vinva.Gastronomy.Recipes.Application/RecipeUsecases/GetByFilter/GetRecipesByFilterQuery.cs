using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Filters;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.GetByFilter
{    
    public readonly record struct GetRecipesByFilterQuery : IRequest<Result<GetRecipesByFilterQueryResponse>>
    {
        public required SearchQuery Query { get; init; }
    }
}
