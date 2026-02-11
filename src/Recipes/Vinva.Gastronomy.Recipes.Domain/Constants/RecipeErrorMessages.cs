using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Domain.Constants
{
    public class RecipeErrorMessages
    {
        public static string FailedRemoveCategoryFromRecipe(Guid recipeId, Guid categoryId) => $"Не удалось удалить категорию '{categoryId}' из рецепта '{recipeId}'";
        public static string RecipeDoesNotContainsCategory(Guid recipeId, Guid categoryId) => $"Рецепт '{recipeId}' не содержит категорию '{categoryId}'";
        public static string IncorrectTypeOfRecipeCategory(Guid recipeId, Guid categoryId) => $"Некорректный тип категории '{categoryId}' для рецепта '{recipeId}'";
        public static string RecipeCategoryAlreadyExists(Guid recipeId, string name) => $"Рецепт '{recipeId}' уже содержит категорию '{name}'";
        public static string RecipeIngredientAlreadyExists(Guid recipeId, Guid ingredientId) => $"Рецепт '{recipeId}' уже содержит ингредиент '{ingredientId}'";
        public static string IngredientCategoryAlreadyExists(Guid ingredientId, string name) => $"Ингредиент '{ingredientId}' уже содержит категорию '{name}'";
        public static string NotPossibleInheritNotBaseRecipe(Guid recipeId) => $"Невозможно сделать вариацию рецепта '{recipeId}' так как он не является базовым";        
    }
}
