using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.ReorderSteps
{
    public sealed class ReorderStepsCommandHandler : IRequestHandler<ReorderStepsCommand, Result>
    {
        private readonly RecipeDbContext _dbContext;

        public ReorderStepsCommandHandler(RecipeDbContext recipeDbContext)
        {
            _dbContext = recipeDbContext ?? throw new ArgumentNullException(nameof(recipeDbContext));
        }

        public async Task<Result> Handle(ReorderStepsCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var validator = new ReorderStepsCommandValidator();
            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
                return validationResult.HandleValidationErrors();

            var recipe = await _dbContext.Recipes.Include(a => a.Steps)
                .FirstOrDefaultAsync(a => a.Id == command.RecipeId, cancellationToken);

            if (recipe == null)
                throw new BadRequestException(RecipesApplicationErrors.RecipeNotFound.Description);

            try
            {                
                foreach (var stepReorderItem in command.Items)
                    recipe.ChangeStepOrder(stepReorderItem.SeqNumber1,
                        stepReorderItem.SeqNumber2);
            }
            catch (RecipeDomainException ex)
            {
                throw new BadRequestException(ex.Message, ex);
            }

            _dbContext.Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}