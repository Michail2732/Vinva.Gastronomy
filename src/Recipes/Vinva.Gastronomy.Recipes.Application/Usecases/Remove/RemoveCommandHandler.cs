using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Remove
{
    public sealed class RemoveCommandHandler : IRequestHandler<RemoveCommand>
    {
        private readonly RecipeDbContext dbContext;

        public RemoveCommandHandler(RecipeDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(RemoveCommand request, CancellationToken cancellationToken)
        {
            await new RemoveCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

            var recipe = await dbContext.Recipes
                .SingleOrDefaultAsync(item => item.Id == request.RecipeId, cancellationToken)
                ?? throw new KeyNotFoundException($"Recipe '{request.RecipeId}' was not found.");

            var characteristics = await dbContext.CharacteristicRecords
                .Where(record => record.RecipeId == request.RecipeId)
                .ToListAsync(cancellationToken);

            dbContext.CharacteristicRecords.RemoveRange(characteristics);
            dbContext.Recipes.Remove(recipe);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
