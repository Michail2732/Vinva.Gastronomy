using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddCategories
{
    public sealed class AddRecipeCategoriesCommandHandler : BaseRecipeHandler, IRequestHandler<AddRecipeCategoriesCommand>
    {
        private readonly RecipeDbContext _dbContext;

        public AddRecipeCategoriesCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(AddRecipeCategoriesCommand request, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();            

            List<Guid> categoryIds = request.CategoryIds.Distinct().ToList();            
            
            var recipe = await GetRecipeById(_dbContext, request.RecipeId, RecipeIncludes.Categories, ct);

            var categories = await _dbContext.Categories.AsNoTracking()
                .Where(a => request.CategoryIds.Contains(a.Id))
                .ToListAsync(ct);

            foreach (var categoryId in categoryIds)
            {
                if (!categories.Any(a => a.Id == categoryId))
                    throw new BadRequestException(RecipesApplicationErrors.CategoryNotFound(categoryId));
            }

            try
            {
                foreach (var category in categories)                
                    recipe.AddCategory(category);                                
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