using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Common
{
    internal static class RecipeIngredientDtoMapper
    {
        public static RecipeIngredientsSet ToDomain(RecipeIngredientsSetDto dto, Guid? recipeId = null)
        {
            var set = new RecipeIngredientsSet(
                dto.Id ?? GuidProvider.Instance.Generate(),
                recipeId ?? Guid.Empty,
                dto.Name,
                dto.PortionsQuantities.Select(ToDomain))
            {
                PortionsCalculateType = dto.PortionsCalculateType
            };

            foreach (var ingredient in dto.Ingredients)
            {
                set.AddIngredient(new RecipeIngredient(
                    ingredient.Id ?? GuidProvider.Instance.Generate(),
                    ingredient.Name,
                    set.Id,
                    ingredient.Quantities.Select(ToDomain)));
            }

            return set;
        }

        public static RecipeIngredientsSetDto ToDto(RecipeIngredientsSet set)
        {
            return new RecipeIngredientsSetDto
            {
                Id = set.Id,
                Name = set.Name,
                PortionsQuantities = set.PortionsQuantities.Select(ToDto).ToArray(),
                PortionsCalculateType = set.PortionsCalculateType,
                Ingredients = set.Ingredients.Select(ToDto).ToArray(),
            };
        }

        public static RecipeIngredientDto ToDto(RecipeIngredient ingredient)
        {
            return new RecipeIngredientDto
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Quantities = ingredient.Quantities.Select(ToDto).ToArray(),
            };
        }

        private static Quantity ToDomain(QuantityDto dto) => new(dto.Amount, dto.Unit);

        private static QuantityDto ToDto(Quantity quantity) => new()
        {
            Amount = quantity.Amount,
            Unit = quantity.Unit,
        };
    }
}
