using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Remove
{
    public sealed class RemoveIngredientsCommandHandler : IRequestHandler<RemoveIngredientsCommand>
    {
        private readonly RecipeDbContext _dbContext;

        public RemoveIngredientsCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(RemoveIngredientsCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var ingredientIds = request.IngredientIds.Distinct().ToList();
            var ingredients = await _dbContext.Ingredients.Where(a => ingredientIds.Contains(a.Id))
                                .ToListAsync();

            foreach (var ingredientId in ingredientIds)            
                if (!ingredients.Any(a => a.Id == ingredientId))
                    throw new BadRequestException(RecipesApplicationErrors.IngredientNotFound(ingredientId));
            

            _dbContext.Ingredients.RemoveRange(ingredients);
            await _dbContext.SaveChangesAsync(cancellationToken);            
        }
    }
}