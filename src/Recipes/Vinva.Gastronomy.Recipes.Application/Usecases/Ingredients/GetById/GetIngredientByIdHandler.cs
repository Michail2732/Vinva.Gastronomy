using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Recipes.Application.Common.Map;
using Vinva.Gastronomy.Recipes.Application.Constants;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.GetById
{
    public class GetIngredientByIdHandler : IRequestHandler<GetIngredientByIdRequest, GetIngredientByIdResponce>
    {
        private readonly RecipeDbContext _dbContext;        
        private readonly RecipeApplicationMapper _mapper;

        public GetIngredientByIdHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = new RecipeApplicationMapper();
        }

        public async Task<GetIngredientByIdResponce> Handle(GetIngredientByIdRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var ingredient = await _dbContext.Ingredients.Include(a => a.Categories)                                    
                                    .FirstOrDefaultAsync(a => a.Id == request.IngredientId);

            if (ingredient == null)
                throw new NotFoundException(RecipesApplicationErrors.IngredientNotFound(request.IngredientId));

            var result = _mapper.Map(ingredient);

            return new GetIngredientByIdResponce
            {
                Ingredient = result
            };
        }
    }
}
