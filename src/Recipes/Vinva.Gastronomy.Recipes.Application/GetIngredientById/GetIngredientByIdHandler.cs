using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.Common.Map;
using Vinva.Gastronomy.Recipes.Application.GetRecipeByCategory;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.GetIngredientById
{
    public class GetIngredientByIdHandler : IRequestHandler<GetIngredientByIdRequest, Result<GetIngredientByIdResponce>>
    {
        private readonly RecipeDbContext _dbContext;        
        private readonly RecipeApplicationMapper _mapper;

        public GetIngredientByIdHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = new RecipeApplicationMapper();
        }

        public async Task<Result<GetIngredientByIdResponce>> Handle(GetIngredientByIdRequest request, CancellationToken cancellationToken)
        {
            var ingredient = await _dbContext.Ingredients.Include(a => a.Categories)                                    
                                    .FirstOrDefaultAsync(a => a.Id == request.IngredientId);

            if (ingredient == null)
                return Result.Failure<GetIngredientByIdResponce>(Errors.);
        }
    }
}
