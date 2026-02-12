using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveRecipeCategory
{
    public sealed class RemoveRecipeCategoryCommandHandler : IRequestHandler<RemoveRecipeCategoryCommand, Result>
    {
        private readonly RecipeDbContext _dbContext;

        public RemoveRecipeCategoryCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result> Handle(RemoveRecipeCategoryCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var validator = new RemoveRecipeCategoryCommandValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                return validationResult.HandleValidationErrors<Result>();

            List<Guid> categoryIds = request.CategoryIds.Distinct().ToList();

            var recipe = await _dbContext.Recipes.Include(a => a.Categories)
                .FirstOrDefaultAsync(a => a.Id == request.RecipeId, cancellationToken);

            if (recipe == null)
                throw new BadRequestException(RecipesApplicationErrors.RecipeNotFound.Description);

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
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}