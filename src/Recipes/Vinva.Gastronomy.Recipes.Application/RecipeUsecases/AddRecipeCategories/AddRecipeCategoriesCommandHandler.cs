using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddRecipeCategories
{
    public sealed class AddRecipeCategoriesCommandHandler : IRequestHandler<AddRecipeCategoriesCommand, Result>
    {
        private readonly RecipeDbContext _dbContext;

        public AddRecipeCategoriesCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result> Handle(AddRecipeCategoriesCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var validator = new AddRecipeCategoriesCommandValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                return validationResult.HandleValidationErrors<Result>();

            List<Guid> categoryIds = request.CategoryIds.Distinct().ToList();            

            var recipe = await _dbContext.Recipes.Include(a => a.Categories)
                .FirstOrDefaultAsync(a => a.Id == request.RecipeId, cancellationToken);

            if (recipe == null)
                throw new BadRequestException(RecipesApplicationErrors.RecipeNotFound.Description);

            var categories = await _dbContext.Categories.AsNoTracking()
                .Where(a => request.CategoryIds.Contains(a.Id))
                .ToListAsync(cancellationToken);

            foreach (var categoryId in categoryIds)
            {
                if (!categories.Any(a => a.Id == categoryId))
                    throw new BadRequestException(RecipesApplicationErrors.CategoryNotFound(categoryId).Description);
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
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}