using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.CategoryUsecases.RemoveCategory
{
    public sealed class RemoveCategoryCommandHandler : IRequestHandler<RemoveCategoryCommand, Result>
    {
        private readonly RecipeDbContext _dbContext;

        public RemoveCategoryCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var validator = new RemoveCategoryCommandValidator();
            var validResult = validator.Validate(request);
            if (!validResult.IsValid)
                return validResult.HandleValidationErrors();

            var categoryIds = request.CategoryIds.Distinct().ToList();

            var categories = await _dbContext.Categories.Where(a => categoryIds.Contains(a.Id))
                                .ToListAsync(cancellationToken);

            foreach (var categoryId in request.CategoryIds)            
                if (!categories.Any(a => a.Id == categoryId))
                    throw new BadRequestException(RecipesApplicationErrors.CategoryNotFound(categoryId).Description);

            _dbContext.Categories.RemoveRange(categories);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}