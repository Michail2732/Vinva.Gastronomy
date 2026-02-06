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

namespace Vinva.Gastronomy.Recipes.Application.GetRecipeByCategory
{
    public class GetRecipeByCategoryHandler : IRequestHandler<GetRecipeByCategoryRequest, Result<GetRecipeByCategoryResponce>>
    {
        private readonly RecipeDbContext _dbContext;
        private readonly CategoryFilterExpressionBuilder _filterBuilder;
        private readonly RecipeApplicationMapper _mapper;

        public GetRecipeByCategoryHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _filterBuilder = new CategoryFilterExpressionBuilder();
            _mapper = new RecipeApplicationMapper();
        }

        public async Task<Result<GetRecipeByCategoryResponce>> Handle(GetRecipeByCategoryRequest request, CancellationToken cancellationToken)
        {
            var exprFilter = _filterBuilder.CreateExpression(request);

            var recipes = await _dbContext.Recipes.Include(a => a.Ingredients)
                                    .Include(a => a.Categories)
                                    .Include(a => a.Steps)
                                    .Where(exprFilter)
                                    .ToListAsync();
            var result = _mapper.Map(recipes);

            return Result.Success(new GetRecipeByCategoryResponce
            {
                Recipes = result
            });
        }        
    }
}
