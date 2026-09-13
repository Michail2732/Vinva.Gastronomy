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
                .Skip((int)request.Skip)
                .Take(request.Take > 0 ? request.Take : int.MaxValue)
                .ToListAsync(cancellationToken);

            var recipeIds = recipes.Select(recipe => recipe.Id).ToArray();
            var records = await dbContext.CharacteristicRecords
                .Where(record => recipeIds.Contains(record.RecipeId))
                .ToListAsync(cancellationToken);

            foreach (var group in records.GroupBy(record => record.RecipeId))
            {
                var recipe = recipes.Single(recipe => recipe.Id == group.Key);
                foreach (var record in group)
                    recipe.AddCharacteristic(CharacteristicRecordMapper.ToDomain(record));
            }

            return new SearchRecipesByCharacteristicsResponse
            {
                Recipes = RecipeApplicationMapper.Map(recipes)
            };
        }
    }
}
