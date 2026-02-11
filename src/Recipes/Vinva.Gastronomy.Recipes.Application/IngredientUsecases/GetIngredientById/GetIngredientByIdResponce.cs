using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.IngredientUsecases.GetIngredientById
{
    public readonly record struct GetIngredientByIdResponce
    {
        public IngredientDto Ingredient { get; init; }
    }
}
