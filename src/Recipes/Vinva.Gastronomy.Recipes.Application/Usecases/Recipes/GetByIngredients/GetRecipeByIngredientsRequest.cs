using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByIngredients
{
    public readonly record struct GetRecipeByIngredientsRequest : IRequest<GetRecipeByIngredientsResponce>
    {
        public List<Guid>? Include { get; init; }
        public List<Guid>? Exclude { get; init; }
        public bool IncludeLogicAnd { get; init; }
    }
}
