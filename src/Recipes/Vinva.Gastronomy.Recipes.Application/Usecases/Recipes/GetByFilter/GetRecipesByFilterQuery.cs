using MediatR;
using Vinva.Gastronomy.Common.Models.Search;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByFilter
{    
    public readonly record struct GetRecipesByFilterQuery : IRequest<GetRecipesByFilterQueryResponse>
    {
        public required SearchQuery Query { get; init; }
    }
}
