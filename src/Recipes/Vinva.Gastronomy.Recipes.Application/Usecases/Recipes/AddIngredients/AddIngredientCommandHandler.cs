using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Models;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddIngredients
{
    public sealed class AddIngredientCommandHandler : BaseRecipeHandler, IRequestHandler<AddIngredientCommand>
    {
        private readonly RecipeDbContext _dbContext;

        public AddIngredientCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(AddIngredientCommand command, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var recipe = await GetRecipeById(_dbContext, command.RecipeId, RecipeIncludes.Ingredients, ct);

            try
            {
                var recipeIngredients = await PrepareRecipeIngredientsAsync(command, ct);
                foreach (var recipeIngredient in recipeIngredients)                
                    recipe.AddIngredient(recipeIngredient);
            }
            catch (RecipeDomainException ex)
            {
                throw new BadRequestException(ex.Message, ex);
            }
            
            _dbContext.Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync(ct);            
        }

        private async Task<IList<RecipeIngredient>> PrepareRecipeIngredientsAsync(AddIngredientCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = new List<RecipeIngredient>();
            var ingredientIds = request.Ingredients.Select(a => a.IngredientId).ToList();
            var ingredients = await _dbContext.Ingredients.Where(a => ingredientIds.Contains(a.Id))
                .ToListAsync(cancellationToken);

            foreach (var recipeIngredientDto in request.Ingredients)
            {
                var ingredient = ingredients.FirstOrDefault(a => a.Id == recipeIngredientDto.IngredientId);
                if (ingredient == null)
                    throw new BadRequestException(RecipesApplicationErrors.IngredientNotFound(recipeIngredientDto.IngredientId).Description);

                var ingredientQuantityList = recipeIngredientDto.Quantities
                    .Select(a => new IngredientQuantity
                    {
                        Quantity = a.Quantity,
                        Measure = a.Measure,
                    }).ToList();
                var ingredientQuantities = new IngredientQuantities(ingredientQuantityList);
                var newRecipeIngredient = ingredient.ToRecipeIngredient(request.RecipeId, ingredientQuantities, recipeIngredientDto.IsRequired);

                result.Add(newRecipeIngredient);
            }
            return result;
        }
    }
}