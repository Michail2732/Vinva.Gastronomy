using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.GetRecipeByCategory
{
    public readonly record struct GetRecipeByCategoryRequest : IRequest<Result<GetRecipeByCategoryResponce>>
    {
        public List<Guid>? Include { get; init; }
        public List<Guid>? Exclude { get; init; }
        public bool IncludeLogicAnd { get; init; }
    }
}
