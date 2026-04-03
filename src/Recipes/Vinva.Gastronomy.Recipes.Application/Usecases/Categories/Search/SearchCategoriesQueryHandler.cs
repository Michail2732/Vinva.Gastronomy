using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Models.Search;
using Vinva.Gastronomy.Recipes.Application.Common.Map;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Categories.Get
{
    public sealed class SearchCategoriesQueryHandler : IRequestHandler<SearchCategoriesQuery, SearchCategoriesQueryResponse>
    {
        private readonly RecipeDbContext _dbContext;
        private readonly QueryExpressionBuilder<Category> _searchBuilder;
        private readonly RecipeApplicationMapper _mapper;

        public SearchCategoriesQueryHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _searchBuilder = new QueryExpressionBuilder<Category>();
            _mapper = new RecipeApplicationMapper();
        }

        public  async Task<SearchCategoriesQueryResponse> Handle(SearchCategoriesQuery request, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var query = _searchBuilder.BuildQuery(_dbContext.Categories, request.Query);
            var result = await query.ToListAsync(ct);

            return new SearchCategoriesQueryResponse
            {
                Items = _mapper.Map(result)
            };
        }
    }
}
