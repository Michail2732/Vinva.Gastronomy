using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.GetById
{
    public readonly record struct GetIngredientByIdRequest : IRequest<GetIngredientByIdResponce>
    {
        public Guid IngredientId { get; init; }

    }
}
