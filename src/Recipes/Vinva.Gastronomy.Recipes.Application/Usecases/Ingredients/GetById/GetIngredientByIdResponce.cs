using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.GetById
{
    public readonly record struct GetIngredientByIdResponce
    {
        public IngredientDto Ingredient { get; init; }
    }
}
