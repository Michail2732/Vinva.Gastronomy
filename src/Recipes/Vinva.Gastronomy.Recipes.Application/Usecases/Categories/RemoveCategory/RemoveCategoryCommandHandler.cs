using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Categories.RemoveCategory
{
    public sealed class RemoveCategoryCommandHandler : IRequestHandler<RemoveCategoryCommand>
    {
        private readonly RecipeDbContext _dbContext;

        public RemoveCategoryCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();            

            var categoryIds = request.CategoryIds.Distinct().ToList();

            var categories = await _dbContext.Categories.Where(a => categoryIds.Contains(a.Id))
                                .ToListAsync(cancellationToken);

            foreach (var categoryId in request.CategoryIds)            
                if (!categories.Any(a => a.Id == categoryId))
                    throw new BadRequestException(RecipesApplicationErrors.CategoryNotFound(categoryId).Description);

            _dbContext.Categories.RemoveRange(categories);
            await _dbContext.SaveChangesAsync(cancellationToken);            
        }
    }
}