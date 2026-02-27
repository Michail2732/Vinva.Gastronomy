using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Common.Map;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Create
{
    public sealed class CreateIngredientHandler : IRequestHandler<CreateIngredientCommand, CreateIngredientResponse>
    {        
        private readonly RecipeDbContext _dbContext;

        public CreateIngredientHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<CreateIngredientResponse> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var newIngredient = new Ingredient(request.Name, request.Description)
            {
                UsageComment = request.UsageComment
            };

            var result = await _dbContext.Ingredients.AddAsync(newIngredient, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateIngredientResponse
            {
                IngredientId = result.Entity.Id
            };
        }
    }
}
