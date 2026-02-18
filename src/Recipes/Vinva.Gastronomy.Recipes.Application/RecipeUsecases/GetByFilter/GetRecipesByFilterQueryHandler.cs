using MediatR;
using Vinva.Gastronomy.Recipes.Persistence;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.GetByFilter
{
    public sealed class GetRecipesByFilterQueryHandler : IRequestHandler<GetRecipesByFilterQuery, GetRecipesByFilterQueryResponse>
    {
        private readonly RecipeDbContext _dbContext;

        public GetRecipesByFilterQueryHandler(RecipeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<GetRecipesByFilterQueryResponse> Handle(GetRecipesByFilterQuery request, CancellationToken cancellationToken)
        {
            // Implement your logic here
            throw new NotImplementedException();
        }
    }
}
