using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Exceptions;


using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddSteps
{
    public sealed class AddStepsCommandHandler : BaseRecipeHandler, IRequestHandler<AddStepsCommand>
    {
        private readonly RecipeDbContext _dbContext;
        public AddStepsCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(AddStepsCommand command, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var recipe = await GetRecipeById(_dbContext, command.RecipeId, RecipeIncludes.Steps, ct);

            var orderedSteps = command.Steps.OrderBy(a => a.SeqNumber).ToList();

            foreach (var step in orderedSteps)
            {
                recipe.AddStep(step.Description, step.Comment, step.PhotoId);
            }

            _dbContext.Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync(ct);            
        }
    }
}