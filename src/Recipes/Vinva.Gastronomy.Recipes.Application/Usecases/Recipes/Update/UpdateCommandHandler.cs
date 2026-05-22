using MediatR;
using Vinva.Gastronomy.Common.Models;
using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Application.Common.Map;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Update
{
    public sealed class UpdateCommandHandler : BaseRecipeHandler, IRequestHandler<UpdateCommand>
    {
        private readonly RecipeDbContext _dbContext;        

        public UpdateCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(UpdateCommand request, CancellationToken ct)
        {
            var maper = new RecipeApplicationMapper();

            var recipe = await GetRecipeById(_dbContext, request.Id, RecipeIncludes.All, ct);
            if (request.Name != null)
            {
                recipe.Name = request.Name;
            }
            if (request.Description != null)
            {
                recipe.Description = request.Description;
            }
            if (request.Comment != null)
            {
                recipe.Comment = request.Comment;
            }
            if (request.BaseRecipe != null)
            {
                recipe.BaseRecipe = request.BaseRecipe;
            }
            if (request.CookingComment != null)
            {
                recipe.CookingComment = request.CookingComment;
            }
            if (request.CookingTime != null)
            {
                recipe.CookingTime = request.CookingTime;
            }
            if (request.IngredientComment != null)
            {
                recipe.IngredientComment = request.IngredientComment;
            }
            if (request.StorageComment != null)
            {
                recipe.StorageComment = request.StorageComment;
            }
            if (request.Properties != null)
            {
                recipe.Properties = maper.Map(request.Properties);
            }
            if (request.Ingredients?.Length > 0)
            {
                foreach (RecipeIngredientDto ingredientDto in request.Ingredients)
                {
                    if (ingredientDto.State == DtoState.Change)
                    {
                        recipe.RemoveIngredient(ingredientDto.IngredientId);
                        recipe.AddIngredient(maper.Map(ingredientDto, recipe.Id));
                    }
                    else if (ingredientDto.State == DtoState.New)
                    {                        
                        recipe.AddIngredient(maper.Map(ingredientDto, recipe.Id));
                    }
                    else if (ingredientDto.State == DtoState.Remove)
                    {
                        recipe.RemoveIngredient(ingredientDto.IngredientId);                        
                    }                    
                }                
            }
            if (request.Steps?.Length > 0)
            {
                foreach (RecipeStepDto stepDto in request.Steps)
                {
                    if (stepDto.State == DtoState.Change)
                    {
                        recipe.RemoveStep(stepDto.Id);
                        recipe.AddStep(maper.Map(stepDto, recipe.Id));
                    }
                    else if (stepDto.State == DtoState.New)
                    {
                        recipe.AddStep(maper.Map(stepDto, recipe.Id));
                    }
                    else if (stepDto.State == DtoState.Remove)
                    {
                        recipe.RemoveStep(stepDto.Id);
                    }
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}