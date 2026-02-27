using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveIngredients
{
    public sealed class RemoveIngredientsCommandHandler : BaseRecipeHandler, IRequestHandler<RemoveIngredientsCommand>
    {
        private readonly RecipeDbContext _dbContext;

        public RemoveIngredientsCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(RemoveIngredientsCommand command, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var recipe = await GetRecipeById(_dbContext, command.RecipeId, RecipeIncludes.Ingredients, ct);

            try
            {
                foreach (var ingredientId in command.IngredientIds)
                {
                    recipe.RemoveIngredient(ingredientId);
                }
                _dbContext.Recipes.Update(recipe);
                await _dbContext.SaveChangesAsync(ct);
            }
            catch (RecipeDomainException ex)
            {
                throw new BadRequestException(ex.Message, ex);
            }
            
        }
    }
}