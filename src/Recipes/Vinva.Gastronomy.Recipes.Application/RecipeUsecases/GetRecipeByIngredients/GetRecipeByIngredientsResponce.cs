using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.GetRecipeByIngredients
{
    public readonly record struct GetRecipeByIngredientsResponce
    {
        public List<RecipeDto> Recipes { get; init; }
    }
}
