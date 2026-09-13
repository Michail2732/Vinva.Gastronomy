using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.SearchByCharacteristics
{
    public sealed record SearchRecipesByCharacteristicsResponse
    {
        public IReadOnlyList<RecipeDto> Recipes { get; init; } = [];
    }
}
