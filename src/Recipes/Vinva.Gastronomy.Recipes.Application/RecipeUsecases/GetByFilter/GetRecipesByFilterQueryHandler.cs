using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Filters;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.Common.Map;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.GetByFilter
{
    public sealed class GetRecipesByFilterQueryHandler : IRequestHandler<GetRecipesByFilterQuery, Result<GetRecipesByFilterQueryResponse>>
    {
        private readonly RecipeDbContext _dbContext;

        public GetRecipesByFilterQueryHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result<GetRecipesByFilterQueryResponse>> Handle(GetRecipesByFilterQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetRecipesByFilterQueryValidator();
            var exprBuilder = new QueryExpressionBuilder<Recipe>();
            var mapper = new RecipeApplicationMapper();

            var result = await exprBuilder.BuildQuery(_dbContext.Recipes
                .Include(a => a.Ingredients)
                .Include(a => a.Steps)
                .Include(a => a.Categories), request.Query)
                        .ToListAsync(cancellationToken);
            return new GetRecipesByFilterQueryResponse
            {
                Recipes = mapper.Map(result)
            };
        }
    }
}
