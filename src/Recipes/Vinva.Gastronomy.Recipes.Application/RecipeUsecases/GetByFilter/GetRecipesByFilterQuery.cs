using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Filters;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.GetByFilter
{    
    public record GetRecipesByFilterQuery : IRequest<GetRecipesByFilterQueryResponse>
    {
        public required SearchQuery Filter { get; init; }
    }
}
