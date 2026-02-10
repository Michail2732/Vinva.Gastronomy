using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.CreateRecipe
{
    public class CreateRecipeHandler : IRequestHandler<CreateRecipeCommand, Result<CreateRecipeResponce>>
    {
        private readonly RecipeDbContext _dbContext;

        public CreateRecipeHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result<CreateRecipeResponce>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateRecipeCommandValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
                return validationResult.HandleValidationErrors<CreateRecipeResponce>();

            var newRecipe = new Recipe(request.Name, request.Description, request.BaseRecipe)
            {
                Comment = request.Comment,
                CookingTime = request.CookingTime,
                UsageComment = request.UsageComment,
                StorageComment = request.StorageComment
            };

            var recipeWithSameName = _dbContext.Recipes.FirstOrDefault(a => a.Name == request.Name);
            throw new Exception();
        }
    }
}
