using System;
using System.Collections.Generic;
using System.Text;

namespace Vinva.Gastronomy.Recipes.Domain.Constants
{
    public class RecipeErrorMessages
    {
        public static string RecipeCategoryAlreadyExists(Guid recipeId, string name) => $"Рецепт '{recipeId}' уже содержит категорию '{name}'";
        public static string RecipeIngredientAlreadyExists(Guid recipeId, Guid ingredientId) => $"Рецепт '{recipeId}' уже содержит ингредиент '{ingredientId}'";
        public static string IngredientCategoryAlreadyExists(Guid ingredientId, string name) => $"Ингредиент '{ingredientId}' уже содержит категорию '{name}'";
    }
}
