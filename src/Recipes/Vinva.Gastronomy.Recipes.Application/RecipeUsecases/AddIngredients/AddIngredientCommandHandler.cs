using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Models;
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

        public async Task<Result> Handle(AddIngredientCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var validator = new AddIngredientCommandValidator();
            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
                return validationResult.HandleValidationErrors<Result>();

            var recipe = await _dbContext.Recipes.Include(a => a.Ingredients)
                .FirstOrDefaultAsync(a => a.Id == command.RecipeId, cancellationToken);

            if (recipe == null)
                throw new BadRequestException(RecipesApplicationErrors.RecipeNotFound.Description);
            

            try
            {
                var recipeIngredients = await PrepareRecipeIngredientsAsync(command, cancellationToken);
                foreach (var recipeIngredient in recipeIngredients)                
                    recipe.AddIngredient(recipeIngredient);
            }
            catch (RecipeDomainException ex)
            {
                throw new BadRequestException(ex.Message, ex);
            }
            
            _dbContext.Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
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
                    throw new BadRequestException(RecipesApplicationErrors.IngredientNotFound.Description);

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