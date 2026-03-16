using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByCategory
{
    public readonly record struct GetRecipeByCategoryRequest : IRequest<GetRecipeByCategoryResponce>
    {
        public List<Guid>? Include { get; init; }
        public List<Guid>? Exclude { get; init; }
        public bool IncludeLogicAnd { get; init; }
    }
}
