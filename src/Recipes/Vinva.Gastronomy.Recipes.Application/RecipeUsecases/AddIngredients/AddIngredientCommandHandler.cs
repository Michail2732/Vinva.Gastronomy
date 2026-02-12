using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddIngredients
{
    public sealed class AddIngredientCommandHandler : IRequestHandler<AddIngredientCommand, Result>
    {
        private readonly RecipeDbContext _dbContext;

        public AddIngredientCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result> Handle(AddIngredientCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var validator = new AddIngredientCommandValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                return validationResult.HandleValidationErrors<Result>();

            var recipe = await _dbContext.Recipes.Include(a => a.Ingredients)
                .FirstOrDefaultAsync(a => a.Id == request.RecipeId, cancellationToken);

            if (recipe == null)
                throw new BadRequestException(RecipesApplicationErrors.RecipeNotFound.Description);

            var ingredientIds = request.Ingredients.Select(a => a.IngredientId).ToList();
            await _dbContext.Ingredients.Where(a => ingredientIds.Contains(a.Id)).ToListAsync(cancellationToken);

            foreach (var ingredient in request.Ingredients)
            {
                if (recipe.Ingredients.Any(a => a.IngredientId == ingredient.IngredientId))
                    throw new BadRequestException(RecipesApplicationErrors.CategoryNotFound(categoryId).Description);
            }
        }
    }
}