using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveIngredients
{
    public sealed class RemoveIngredientsCommandHandler : IRequestHandler<RemoveIngredientsCommand, Result>
    {
        private readonly RecipeDbContext _dbContext;

        public RemoveIngredientsCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result> Handle(RemoveIngredientsCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var validator = new RemoveIngredientsCommandValidator();
            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
                return validationResult.HandleValidationErrors<Result>();

            var recipe = await _dbContext.Recipes.Include(a => a.Ingredients)
                .FirstOrDefaultAsync(a => a.Id == command.RecipeId, cancellationToken);

            if (recipe == null)
                throw new BadRequestException(RecipesApplicationErrors.RecipeNotFound.Description);

            try
            {
                foreach (var ingredientId in command.IngredientIds)
                {
                    recipe.RemoveIngredient(ingredientId);
                }
                _dbContext.Recipes.Update(recipe);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (RecipeDomainException ex)
            {
                throw new BadRequestException(ex.Message, ex);
            }

            return Result.Success();
        }
    }
}