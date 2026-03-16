using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;


using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveCategory
{
    public sealed class RemoveRecipeCategoryCommandHandler : BaseRecipeHandler, IRequestHandler<RemoveRecipeCategoryCommand>
    {
        private readonly RecipeDbContext _dbContext;

        public RemoveRecipeCategoryCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(RemoveRecipeCategoryCommand request, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();            

            List<Guid> categoryIds = request.CategoryIds.Distinct().ToList();

            var recipe = await GetRecipeById(_dbContext, request.RecipeId, RecipeIncludes.Categories, ct);

            try
            {
                foreach (var categoryId in categoryIds)
                    recipe.RemoveCategory(categoryId);
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