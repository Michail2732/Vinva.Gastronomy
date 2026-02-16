using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddSteps
{
    public sealed class AddStepsCommandHandler : IRequestHandler<AddStepsCommand, Result>
    {
        private readonly RecipeDbContext _dbContext;
        public AddStepsCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result> Handle(AddStepsCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var validator = new AddStepsCommandValidator();
            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
                return validationResult.HandleValidationErrors();

            var recipe = await _dbContext.Recipes.Include(a => a.Steps)
                .FirstOrDefaultAsync(a => a.Id == command.RecipeId, cancellationToken);

            if (recipe == null)
                throw new BadRequestException(RecipesApplicationErrors.RecipeNotFound.Description);

            var orderedSteps = command.Steps.OrderBy(a => a.SeqNumber).ToList();

            foreach (var step in orderedSteps)
            {
                recipe.AddStep(step.Description, step.Comment, step.PhotoId);
            }

            _dbContext.Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}