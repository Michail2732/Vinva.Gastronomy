using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.Common.Map;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByIngredients
{
    public class GetRecipeByIngredientsHandler : BaseRecipeHandler, IRequestHandler<GetRecipeByIngredientsRequest, GetRecipeByIngredientsResponce>
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IngredientsFilterExpressionBuilder _filterBuilder;
        private readonly RecipeApplicationMapper _mapper;

        public GetRecipeByIngredientsHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _filterBuilder = new IngredientsFilterExpressionBuilder();
            _mapper = new RecipeApplicationMapper();
        }

        public async Task<GetRecipeByIngredientsResponce> Handle(GetRecipeByIngredientsRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var exprFilter = _filterBuilder.CreateExpression(request);

            var recipes = await _dbContext.Recipes.Include(a => a.Ingredients)
                                    .Include(a => a.Categories)
                                    .Include(a => a.Steps)
                                    .Where(exprFilter)
                                    .ToListAsync();

            var result = _mapper.Map(recipes);

            return new GetRecipeByIngredientsResponce
            {
                Recipes = result
            };
        }        
    }
}
