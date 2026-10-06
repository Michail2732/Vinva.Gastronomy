using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Application.Usecases.Common;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Application.Usecases
{
    internal static class RecipeApplicationMapper
    {
        public static List<RecipeDto> Map(IEnumerable<Recipe> recipes)
        {
            return recipes.Select(Map).ToList();
        }

        public static RecipeDto Map(Recipe recipe)
        {
            return new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                BaseRecipe = recipe.BaseRecipe,
                Document = recipe.Document,
                Characteristics = recipe.Characteristics.Select(characteristic => new CharacteristicDto
                {
                    RecipeId = recipe.Id,
                    Name = characteristic.Name,
                    Type = characteristic.Type,
                    Value = characteristic.Value.GetJson(),
                    Measure = characteristic.Measure
                }).ToArray()
                ,Ingredients = recipe.Ingredients.Select(RecipeIngredientDtoMapper.ToDto).ToArray()
            };
        }
    }
}
