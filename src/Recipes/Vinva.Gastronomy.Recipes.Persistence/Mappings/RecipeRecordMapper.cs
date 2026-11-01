using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence.Models;

namespace Vinva.Gastronomy.Recipes.Persistence.Mappings
{
    public static class RecipeRecordMapper
    {
        public static RecipeRecord ToRecord(Recipe recipe)
        {
            return new RecipeRecord
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                BaseRecipe = recipe.BaseRecipe,
                Document = recipe.Document,
                TitleImageId = recipe.TitleImageId,
                GalleryImageIds = recipe.GalleryImageIds,
                Characteristics = recipe.Characteristics.Select(CharacteristicRecordMapper.ToRecord).ToList(),
                IngredientsSets = recipe.IngredientsSets.Select(RecipeIngredientsSetRecordMapper.ToRecord).ToList()
            };
        }

        public static Recipe ToDomain(RecipeRecord record)
        {
            var recipe = new Recipe(record.Id, record.Name, record.Description, record.Document, record.BaseRecipe)
            {
                TitleImageId = record.TitleImageId,
                GalleryImageIds = record.GalleryImageIds
            };

            foreach (var characteristic in record.Characteristics)
                recipe.AddCharacteristic(CharacteristicRecordMapper.ToDomain(characteristic));

            foreach (var ingredientsSet in record.IngredientsSets)
                recipe.AddIngredientsSet(RecipeIngredientsSetRecordMapper.ToDomain(ingredientsSet));

            return recipe;
        }

        public static void ApplyScalars(Recipe recipe, RecipeRecord record)
        {
            record.Name = recipe.Name;
            record.Description = recipe.Description;
            record.BaseRecipe = recipe.BaseRecipe;
            record.Document = recipe.Document;
            record.TitleImageId = recipe.TitleImageId;
            record.GalleryImageIds = recipe.GalleryImageIds;
        }
    }
}
