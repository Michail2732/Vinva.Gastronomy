using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByFilter
{
    public readonly record struct GetRecipesByFilterQueryResponse 
    {
        public List<RecipeDto> Recipes { get; init; }
    }
}
