using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.IngredientUsecases.RemoveIngredients
{
    public sealed class RemoveIngredientsCommandHandler : IRequestHandler<RemoveIngredientsCommand, Result>
    {
        private readonly RecipeDbContext _dbContext;

        public RemoveIngredientsCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result> Handle(RemoveIngredientsCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var validator = new RemoveIngredientsCommandValidator();

            var validResult = validator.Validate(request);
            if (!validResult.IsValid)
                return validResult.HandleValidationErrors();

            var ingredientIds = request.IngredientIds.Distinct().ToList();
            var ingredients = await _dbContext.Ingredients.Where(a => ingredientIds.Contains(a.Id))
                                .ToListAsync();

            foreach (var ingredientId in ingredientIds)            
                if (!ingredients.Any(a => a.Id == ingredientId))
                    throw new BadRequestException(RecipesApplicationErrors.IngredientNotFound(ingredientId).Description);
            

            _dbContext.Ingredients.RemoveRange(ingredients);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}