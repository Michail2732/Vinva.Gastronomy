using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.GetRecipeByCategory;

namespace Vinva.Gastronomy.Recipes.Application.GetRecipeByIngredients
{
    public readonly record struct GetRecipeByIngredientsRequest : IRequest<Result<GetRecipeByIngredientsResponce>>
    {
        public List<Guid>? Include { get; init; }
        public List<Guid>? Exclude { get; init; }
        public bool IncludeLogicAnd { get; init; }
    }
}
