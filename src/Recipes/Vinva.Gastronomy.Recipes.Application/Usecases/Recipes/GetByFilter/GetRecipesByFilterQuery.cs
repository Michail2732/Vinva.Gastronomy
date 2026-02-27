using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Filters;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByFilter
{    
    public readonly record struct GetRecipesByFilterQuery : IRequest<GetRecipesByFilterQueryResponse>
    {
        public required SearchQuery Query { get; init; }
    }
}
