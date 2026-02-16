using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveSteps
{
    public sealed class RemoveStepsCommandHandler : IRequestHandler<RemoveStepsCommand, Result>
    {
        private readonly RecipeDbContext _dbContext;

        public RemoveStepsCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result> Handle(RemoveStepsCommand command, CancellationToken cancellationToken)
        {
            var validator = new RemoveStepsCommandValidator();
            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
                return validationResult.HandleValidationErrors();

            var recipe = await _dbContext.Recipes.Include(a => a.Steps)
                .FirstOrDefaultAsync(a => a.Id == command.RecipeId, cancellationToken);

            if (recipe == null)
                throw new BadRequestException(RecipesApplicationErrors.RecipeNotFound.Description);

            try
            {                
                foreach (var seqNumber in command.SeqNumbers)
                    recipe.RemoveStep(seqNumber);
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