using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Exceptions;
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

        public async Task Handle(RemoveCategoryCommand request, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();            

            var categoryId = request.CategoryId;

            var category = await _dbContext.Categories.FirstOrDefaultAsync(a => a.Id == categoryId, ct);

            if (category == null)
                throw new NotFoundException(RecipesApplicationErrors.CategoryNotFound(categoryId));

            category.Delete();
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync(ct);            
        }
    }
}