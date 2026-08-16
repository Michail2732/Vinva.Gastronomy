using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Remove
{
    public sealed class RemoveIngredientsCommandHandler : IRequestHandler<RemoveIngredientCommand>
    {
        private readonly RecipeDbContext _dbContext;

        public RemoveIngredientsCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(RemoveIngredientCommand request, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var ingredientId = request.IngredientId;
            var ingredient = await _dbContext.Ingredients.FirstOrDefaultAsync(a => a.Id == ingredientId, ct);                                

            if (ingredient == null)            
                 throw new NotFoundException(RecipesApplicationErrors.IngredientNotFound(ingredientId));
            
            _dbContext.Ingredients.Remove(ingredient);
            await _dbContext.SaveChangesAsync(ct);            
        }
    }
}