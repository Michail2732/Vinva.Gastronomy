using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Exceptions;


using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.ReorderSteps
{
    public sealed class ReorderStepsCommandHandler : BaseRecipeHandler, IRequestHandler<ReorderStepsCommand>
    {
        private readonly RecipeDbContext _dbContext;

        public ReorderStepsCommandHandler(RecipeDbContext recipeDbContext)
        {
            _dbContext = recipeDbContext ?? throw new ArgumentNullException(nameof(recipeDbContext));
        }

        public async Task Handle(ReorderStepsCommand command, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var recipe = await GetRecipeById(_dbContext, command.RecipeId, RecipeIncludes.Steps, ct);

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
            await _dbContext.SaveChangesAsync(ct);            
        }
    }
}