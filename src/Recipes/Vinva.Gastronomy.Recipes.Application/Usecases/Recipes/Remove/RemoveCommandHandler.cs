using MediatR;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Remove
{
    public sealed class RemoveCommandHandler : BaseRecipeHandler, IRequestHandler<RemoveCommand>
    {
        private readonly RecipeDbContext _dbContext;


        public RemoveCommandHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task Handle(RemoveCommand request, CancellationToken cancellationToken)
        {
            var recipe = await GetRecipeById(_dbContext, request.RecipeId, RecipeIncludes.None, cancellationToken);

            recipe.Delete();

            _dbContext.Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}