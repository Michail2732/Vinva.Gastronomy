using MediatR;
using Vinva.Gastronomy.Common.Models.Search;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Categories.Get
{
    
    public record SearchCategoriesQuery() : IRequest<SearchCategoriesQueryResponse>
    {
        public required SearchQuery Query { get; init; }
    }
}
