using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Recipes.Application.Usecases;
using Vinva.Gastronomy.Recipes.Persistence;
using Vinva.Gastronomy.Recipes.Persistence.Mappings;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.SearchByCharacteristics
{
    public sealed class SearchRecipesByCharacteristicsQueryHandler
        : IRequestHandler<SearchRecipesByCharacteristicsQuery, SearchRecipesByCharacteristicsResponse>
    {
        private readonly RecipeDbContext dbContext;

        public SearchRecipesByCharacteristicsQueryHandler(RecipeDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<SearchRecipesByCharacteristicsResponse> Handle(
            SearchRecipesByCharacteristicsQuery request,
            CancellationToken cancellationToken)
        {
            var validator = new SearchRecipesByCharacteristicsQueryValidator();
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var recipes = await RecipeCharacteristicQueryBuilder.Apply(
                    dbContext.Recipes,
                    dbContext.CharacteristicRecords,
                    request.Filters)
                .Include(recipe => recipe.Characteristics)
                .Include(recipe => recipe.IngredientsSets)
                    .ThenInclude(set => set.Ingredients)
                .AsSplitQuery()
                .Skip((int)request.Skip)
                .Take(request.Take > 0 ? request.Take : int.MaxValue)
                .ToListAsync(cancellationToken);

            return new SearchRecipesByCharacteristicsResponse
            {
                Recipes = RecipeApplicationMapper.Map(recipes.Select(RecipeRecordMapper.ToDomain))
            };
        }
    }
}
