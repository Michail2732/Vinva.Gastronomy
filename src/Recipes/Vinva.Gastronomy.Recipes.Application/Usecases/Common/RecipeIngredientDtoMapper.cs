using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Common
{
    internal static class RecipeIngredientDtoMapper
    {
        public static RecipeIngredient ToDomain(RecipeIngredientDto dto, Guid? recipeId = null)
        {
            return new RecipeIngredient(
                recipeId ?? Guid.Empty,
                dto.Name,
                dto.Quantities.Select(quantity => new RecipeIngredientQuantity(quantity.Amount, quantity.Unit)),
                dto.Part);
        }

        public static RecipeIngredientDto ToDto(RecipeIngredient ingredient)
        {
            return new RecipeIngredientDto
            {
                Name = ingredient.Name,
                Part = ingredient.Part,
                Quantities = ingredient.Quantities
                    .Select(quantity => new RecipeIngredientQuantityDto
                    {
                        Amount = quantity.Amount,
                        Unit = quantity.Unit,
                    })
                    .ToArray(),
            };
        }
    }
}
