using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveSteps
{
    public sealed class RemoveStepsCommandHandler : BaseRecipeHandler, IRequestHandler<RemoveStepsCommand>
    {
        private readonly RecipeDbContext _dbContext;

        public RemoveStepsCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(RemoveStepsCommand command, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var recipe = await GetRecipeById(_dbContext, command.RecipeId, RecipeIncludes.Steps, ct);

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
            await _dbContext.SaveChangesAsync(ct);            
        }
    }
}