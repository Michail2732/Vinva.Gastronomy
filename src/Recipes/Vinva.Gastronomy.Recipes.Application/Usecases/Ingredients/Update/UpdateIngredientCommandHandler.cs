using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Update
{
    public sealed class UpdateIngredientCommandHandler : IRequestHandler<UpdateIngredientCommand>
    {
        public Task Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}