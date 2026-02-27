using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Create
{
    public class CreateRecipeHandler : BaseRecipeHandler, IRequestHandler<CreateRecipeCommand, CreateRecipeResponce>
    {
        private readonly RecipeDbContext _dbContext;

        public CreateRecipeHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<CreateRecipeResponce> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();            

            var newRecipe = new Recipe(request.Name, request.Description, request.BaseRecipe)
            {
                Comment = request.Comment,
                CookingTime = request.CookingTime,
                UsageComment = request.UsageComment,
                StorageComment = request.StorageComment
            };


            var result = await _dbContext.Recipes.AddAsync(newRecipe, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateRecipeResponce
            {
                RecipeId = result.Entity.Id,
                Name = result.Entity.Name
            };
        }
    }
}
