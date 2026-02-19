using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.IngredientUsecases.Update
{
    internal sealed class UpdateIngredientCommandHandler : IRequestHandler<UpdateIngredientCommand, Result>
    {
        public Task<Result> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}