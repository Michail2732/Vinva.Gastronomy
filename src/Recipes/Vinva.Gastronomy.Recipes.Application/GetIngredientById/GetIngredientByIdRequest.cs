using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.GetIngredientById
{
    public readonly record struct GetIngredientByIdRequest : IRequest<Result<GetIngredientByIdResponce>>
    {
        public Guid IngredientId { get; init; }

    }
}
