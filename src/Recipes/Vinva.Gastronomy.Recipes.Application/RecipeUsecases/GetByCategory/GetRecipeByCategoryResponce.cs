using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.GetRecipeByCategory
{
    public readonly record struct GetRecipeByCategoryResponce
    {
        public List<RecipeDto> Recipes { get; init; }
    }
}
